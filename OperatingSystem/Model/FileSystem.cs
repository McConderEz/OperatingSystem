using System.IO;
using System.Text;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Файловая система NTFS
    /// </summary>
    public class FileSystem : IFileSystem
    {
        //TODO:Создать механику Журналирование(Логгирование действий файловой системы и возможность отката)
        public static string FileSystemPath { get; private set; } // Путь файловой системы
        public readonly SuperBlock superBlock;
        public readonly MFT_Table mftTable;

        public FileSystem()
        {
            if (!Exists(@"D:\NTFS")) // Если директория файловой системы не создана, то форматируем
            {
                superBlock = new SuperBlock();
                mftTable = new MFT_Table();
                Formatting();
            }
            superBlock = new SuperBlock();
            mftTable = new MFT_Table();
            //TODO:Если корневая директория существует, значит подгружать метаданные из JSON 
        }

        public void CreateDirectory(string fileName)
        {            
            throw new NotImplementedException();
        }


        public void CreateFile(string fileName)
        {
            string path = $@"D:\NTFS\{fileName}";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                
                mftTable.Add(fileName, new FileInfo(path).FullName ,FileType.File);                
            }
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="path"></param>
        public void Delete(string path)
        {
            if(File.Exists(path)) 
            {
                mftTable.Delete(new FileInfo(path).FullName, superBlock);//Удаление записи из MFT
                File.Delete(path);
            }
        }

        public void Dispose()
        { 

        }

        public bool Exists(string path)
        {
            return ProcessDirectory(path, "D:\\", 1 ,5);
        }      

        public static bool ProcessDirectory(string targetDirectory, string searchDirectory, int depth,int maxDepth)
        {
            if(string.Equals(targetDirectory, searchDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return true; //Указанная директория найдена
            }

            if(depth >= maxDepth)
            {
                return false; // Достигнута максимальная допустимая глубина поиска, а директория так и не была найдена
            }

            try
            {
                string[] subdirectoryEntries = Directory.GetDirectories(searchDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    if (ProcessDirectory(targetDirectory, subdirectory,depth+1,maxDepth))
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
        public StringBuilder ReadFile(string path)
        {
            var data = new StringBuilder();
            using(StreamReader streamReader = new StreamReader(path))
            {
                data.AppendLine(streamReader.ReadToEnd());
            }

            FileInfo fileInfo = new FileInfo(path);
            mftTable.Edit(path, (uint)fileInfo.Length);
            return data;
        }

        /// <summary>
        /// Запись данных в файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="data">Данные на ввод</param>
        /// <exception cref="FileLoadException"></exception>
        public void WriteFile(string fileName, StringBuilder data)
        {
            try
            {
                var mftItem = mftTable.Entries.SingleOrDefault(x => x.Header.Signature == fileName); //Проверка на наличие записи в MFT 

                if (mftItem == null)
                {
                    throw new FileLoadException("Файл, куда вы хотите записать информацию, не существует!", nameof(fileName));
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
                            superBlock.MarkClusterAsUsed(dataBytes,i);
                            mftItem.Attributes.indexesOnClusterBitmap.Add(new Indexer(i));
                            WriteBytesToFile(fileName, stream, dataBytes, dataSize);
                            break;
                        }
                        else if (superBlock.IsClusterFree(i) && dataSize > 4096) // Запись данных в файл, размер которых больше одного кластера(4кб)
                        {
                            superBlock.MarkClustersAsUsedForLargeFile(mftItem, dataBytes, dataSize, i);
                            WriteBytesToFile(fileName, stream, dataBytes, dataSize);
                            break;
                        }


                    }
                }

                FileInfo fileInfo = new FileInfo(fileName);
                mftTable.Edit(fileName, (uint)fileInfo.Length);
            }
            catch (Exception e) { }
        }

        

        /// <summary>
        /// Очистка указателей на битовую карту кластеров записи MFT
        /// </summary>
        /// <param name="mftItem"></param>
        private void ReWriteIndexesOfMFTEntry(MFT_Entry? mftItem)
        {
            var indexes = mftItem.Attributes.indexesOnClusterBitmap;
            for (int i = 0; i < indexes.Count; i++)
            {
                for (var j = 0; j < superBlock.ClusterUnitSize; j++)
                {
                    superBlock.ClusterBitmap[indexes[i].index][j] = 0;
                }
            }
            mftItem.Attributes.indexesOnClusterBitmap.Clear();
        }

        /// <summary>
        /// Запись данных в файл
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <param name="dataBytes"></param>
        /// <param name="dataSize"></param>
        private static void WriteBytesToFile(string fileName, MemoryStream stream, byte[] dataBytes, int dataSize)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(dataBytes);
                using (FileStream fileStream = new FileStream($@"D:\NTFS\{fileName}", FileMode.Open, FileAccess.Write
                    , FileShare.None, dataSize, FileOptions.WriteThrough))
                {
                    stream.WriteTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Создание корневого каталога
        /// </summary>
        public void Formatting()
        {
            FileSystemPath = @"D:\NTFS";
            Directory.CreateDirectory(FileSystemPath);
        }

        public void CopyTo(string path)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(string path)
        {
            throw new NotImplementedException();
        }
    }
}