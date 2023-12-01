using OperatingSystem.Controller;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OperatingSystem.Model.FileSystemEnteties
{
    //TODO:Оптимизировать сохранение в JSON состояние ОС при большом размере битовой карты кластеров
    //Вариант решения:Поделить битовую карту карту кластеров на одинаковое количество отрезков в файловом эквиваленте и работать с нужными,
    //не перегружая систему чтением до конца карты без информации

    //TODO:Сделать перемещение(если реализована многоуровневая фс), копирование файла
    //TODO:Сделать добавление,удаление,перемещение,переименование, копирование каталога (сложно)
    //TODO:Асинхронный доступ к файлам

    /// <summary>
    /// Файловая система NTFS
    /// </summary>
    [DataContract]
    public class FileSystem : ControllerSaveBase, IFileSystem
    {
        //TODO:Создать механику Журналирование(Логгирование действий файловой системы и возможность отката)
        [DataMember]
        public static string FileSystemPath { get; private set; } // Путь файловой системы
        [DataMember]
        public readonly SuperBlock superBlock;
        [DataMember]
        public readonly MFT_Table mftTable;

        private const string MFT_FILE_NAME = "mft.json";// Путь к служебному файлу, хранящим состояние MFT в NTFS
        private const string SUPERBLOCK_FILE_NAME = "superblock.json";// Путь к служебному файлу, хранящим состояние SUPERBLOCKа в NTFS

        [JsonConstructor]
        public FileSystem()
        {
            if (Exists(@"D:\NTFS")) // Если директория файловой системы не создана, то форматируем
            {
                superBlock = GetSuperblock();
                mftTable = GetMFT();
            }
            else
            {
                superBlock = new SuperBlock();
                mftTable = new MFT_Table();
                Formatting();
                Save();
            }
        }

        public void CreateDirectory(string fileName)
        {
            throw new NotImplementedException();
        }


        public void CreateFile(string fullFilePath)
        {
            try
            {                
                if (!File.Exists(fullFilePath))
                {
                    File.Create(fullFilePath).Close();

                    mftTable.Add(fullFilePath, new FileInfo(fullFilePath).FullName, FileType.File);
                    Save();
                }
            }
            catch(Exception ex) { }
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="path"></param>
        public void Delete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    mftTable.Delete(new FileInfo(path).FullName, superBlock);//Удаление записи из MFT
                    File.Delete(path);
                    Save();
                }
            } catch(Exception ex) { }
        }

        public void Dispose()
        {

        }

        public bool Exists(string path)
        {
            return ProcessDirectory(path, "D:\\", 1, 5);
        }

        public static bool ProcessDirectory(string targetDirectory, string searchDirectory, int depth, int maxDepth)
        {
            if (string.Equals(targetDirectory, searchDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return true; //Указанная директория найдена
            }

            if (depth >= maxDepth)
            {
                return false; // Достигнута максимальная допустимая глубина поиска, а директория так и не была найдена
            }

            try
            {
                string[] subdirectoryEntries = Directory.GetDirectories(searchDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    if (ProcessDirectory(targetDirectory, subdirectory, depth + 1, maxDepth))
                    {
                        FileSystemPath = subdirectory;//Изменение ожидаемого пути файловой системы
                        return true; //Найдена в поддиректориях
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {

            }

            return false; // Директория не найдена
        }

        /// <summary>
        /// Вывод файлов каталога
        /// </summary>
        /// <param name="path">Путь к каталогу</param>
        /// <returns></returns>
        public ICollection<string> GetEntities(string path)
        {
            return mftTable.Entries
                .Where(entry => entry.Attributes.ParentsDirectory.Equals(path))
                .Select(entry => entry.Attributes.NameData)
                .ToList();
        }

        /// <summary>
        /// Чтение данных файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public StringBuilder? ReadFile(string path)
        {
            try
            {
                var data = new StringBuilder();
                using (StreamReader streamReader = new StreamReader(path))
                {
                    data.AppendLine(streamReader.ReadToEnd());
                }

                FileInfo fileInfo = new FileInfo(path);
                mftTable.Edit(path, (uint)fileInfo.Length,fileInfo);
                Save();
                return data;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        /// <summary>
        /// Запись данных в файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="data">Данные на ввод</param>
        /// <exception cref="FileLoadException"></exception>
        public void WriteFile(string fullPath, StringBuilder data)
        {
            try
            {
                var mftItem = mftTable.Entries.SingleOrDefault(x => x.Attributes.FullPath == fullPath); //Проверка на наличие записи в MFT 

                if (mftItem == null)
                {
                    throw new FileLoadException("Файл, куда вы хотите записать информацию, не существует!", nameof(fullPath));
                }
                else if (mftItem.Attributes.indexesOnClusterBitmap.Count != 0)
                {
                    ReWriteIndexesOfMFTEntry(mftItem);// Удаление индексов на битовую карту в записи ввиду перезаписи информации
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data.ToString());
                    int dataSize = dataBytes.Length;
                    for (var i = 0; i < superBlock.ClusterBitmap.Length; i++)
                    {
                        if (superBlock.IsClusterFree(i) && dataSize <= 4096) // Запись данных в файл, размер которых не превышает размер одного кластера(4кб)
                        {
                            superBlock.MarkClusterAsUsed(dataBytes, i);
                            mftItem.Attributes.indexesOnClusterBitmap.Add(new Indexer(i));
                            WriteBytesToFile(fullPath, stream, dataBytes, dataSize);
                            break;
                        }
                        else if (superBlock.IsClusterFree(i) && dataSize > 4096) // Запись данных в файл, размер которых больше одного кластера(4кб)
                        {
                            superBlock.MarkClustersAsUsedForLargeFile(mftItem, dataBytes, dataSize, i);
                            WriteBytesToFile(fullPath, stream, dataBytes, dataSize);
                            break;
                        }


                    }
                }

                FileInfo fileInfo = new FileInfo(fullPath);
                mftTable.Edit(fullPath, (uint)fileInfo.Length,fileInfo);
                Save();
            }
            catch (Exception e) { }
        }



        /// <summary>
        /// Очистка указателей на битовую карту кластеров записи MFT
        /// </summary>
        /// <param name="mftItem"></param>
        private void ReWriteIndexesOfMFTEntry(MFT_Entry? mftItem)
        {
            try
            {
                var indexes = mftItem.Attributes.indexesOnClusterBitmap;
                for (int i = 0; i < indexes.Count; i++)
                {
                    for (var j = 0; j < superBlock.ClusterUnitSize; j++)
                    {
                        superBlock.ClusterBitmap[indexes[i].Index][j] = 0;
                    }
                }
                mftItem.Attributes.indexesOnClusterBitmap.Clear();
            }
            catch (Exception e) { }
        }

        /// <summary>
        /// Запись данных в файл
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <param name="dataBytes"></param>
        /// <param name="dataSize"></param>
        private static void WriteBytesToFile(string fullPath, MemoryStream stream, byte[] dataBytes, int dataSize)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(dataBytes);
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Write
                        , FileShare.None, dataSize, FileOptions.WriteThrough))
                    {
                        stream.WriteTo(fileStream);
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Загрузка сохранённого состояния MFT
        /// </summary>
        /// <returns></returns>
        private MFT_Table GetMFT()
        {
            return Load<MFT_Table>(MFT_FILE_NAME).Result ?? new MFT_Table();
        }

        /// <summary>
        /// Загрузка сохранённого состояния Superblock`а
        /// </summary>
        /// <returns></returns>
        private SuperBlock GetSuperblock()
        {
            return Load<SuperBlock>(SUPERBLOCK_FILE_NAME).Result ?? new SuperBlock();
        }

        /// <summary>
        /// Сохранение состояний MFT и Superblock`а
        /// </summary>
        private async void Save()
        {
            await Save(MFT_FILE_NAME, mftTable);
            await Save(SUPERBLOCK_FILE_NAME, superBlock);
        }

        /// <summary>
        /// Создание корневого каталога
        /// </summary>
        public void Formatting()
        {
            FileSystemPath = @"D:\NTFS";
            Directory.CreateDirectory(FileSystemPath);
        }

        /// <summary>
        /// Копировать файл
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CopyTo(string oldFilePath,string newFilePath)
        {
            try
            {
                if (File.Exists(oldFilePath))
                {
                    CreateFile(newFilePath);
                    FileInfo fileInfoNewFile = new FileInfo(newFilePath);

                    if(mftTable.Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(oldFilePath)).Attributes.Length > 0)
                    {
                        WriteFile(newFilePath, ReadFile(oldFilePath));
                    }
                    Save();
                }
            }
            catch(Exception ex) { }
        }

        public void MoveTo(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Переименовать файл
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <exception cref="FileLoadException"></exception>
        public void Rename(string oldName, string newName)
        {
            string path = @"D:\NTFS\";
            if (File.Exists(path + oldName))
            {
                File.Move(path + oldName, path + newName);
                var mftItem = mftTable.Entries.SingleOrDefault(x => x.Header.Signature == oldName); //Проверка на наличие записи в MFT 
                path += newName;
                if (mftItem == null)
                {
                    throw new FileLoadException("Файл, куда вы хотите записать информацию, не существует!", nameof(oldName));
                }
                else 
                {
                    FileInfo fileInfo = new FileInfo(path);
                    string oldPath = $@"D:\NTFS\{oldName}";
                    mftTable.Edit(oldPath, (uint)fileInfo.Length, fileInfo);
                    Save();
                }
            }
        }
    }
}