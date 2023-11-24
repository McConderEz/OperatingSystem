using System.Text;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Файловая система NTFS
    /// </summary>
    public class FileSystem : IFileSystem
    {
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
                mftTable.Add(fileName, FileType.File);                
            }
        }

        public void Delete(string path)
        {
            throw new NotImplementedException();
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

        public ICollection<string> GetEntities(string path)
        {
            throw new NotImplementedException();
        }

        public StringBuilder ReadFile(string path)
        {
            var data = new StringBuilder();
            using(StreamReader streamReader = new StreamReader(path))
            {
                data.AppendLine(streamReader.ReadToEnd());
            }
            return data;
        }

        public void WriteFile(string fileName, StringBuilder data)
        {
            try
            {
                var mftItem = mftTable.Entries.SingleOrDefault(x => x.Header.Signature == fileName);

                if (mftItem == null)
                {
                    throw new FileLoadException("Файл, куда вы хотите записать информацию, не существует!", nameof(fileName));
                }
                else if (mftItem.Attributes.indexesOnClusterBitmap.Count != 0)
                {
                    ReWriteIndexesOfMFTEntry(mftItem);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data.ToString());
                    int dataSize = dataBytes.Length;
                    for (var i = 0; i < superBlock.ClusterBitmap.Length; i++)
                    {
                        if (superBlock.IsClusterFree(i) && dataSize <= 4096)
                        {
                            for (int j = 0; j < dataBytes.Length; j++)
                            {
                                superBlock.ClusterBitmap[i][j] = dataBytes[j];
                            }
                            mftItem.Attributes.indexesOnClusterBitmap.Add(new Indexer(i));

                            WriteBytesToFile(fileName, stream, dataBytes, dataSize);
                            break;
                        }
                        else if (superBlock.IsClusterFree(i) && dataSize > 4096)
                        {
                            int l = 0; // Индекс массива данных на запись 
                            for (int j = i; j < superBlock.ClusterBitmap.Length; j++)
                            {
                                if (superBlock.IsClusterFree(j))
                                {
                                    for (int k = 0; k < dataBytes.Length; k++)
                                    {
                                        if (k < 4096 && l < dataBytes.Length) // Запись байт в кластер
                                        {
                                            superBlock.ClusterBitmap[j][k] = dataBytes[l++];
                                            dataSize--;
                                        }
                                        else
                                        {
                                            // Кластер заполнен и MFT запись получает индексы на область карты кластеров, принадлежащие данному файлу
                                            mftItem.Attributes.indexesOnClusterBitmap.Add(new Indexer(j));
                                            break;
                                        }
                                    }
                                }

                                if (dataSize == 0) // Вся информация записана в кластеры
                                {
                                    break;
                                }

                            }
                            WriteBytesToFile(fileName, stream, dataBytes, dataSize);
                            break;
                        }


                    }
                }
            }
            catch (Exception e) { }
        }

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

        public void Formatting()
        {
            FileSystemPath = @"D:\NTFS";
            Directory.CreateDirectory(FileSystemPath);
        }
    }
}