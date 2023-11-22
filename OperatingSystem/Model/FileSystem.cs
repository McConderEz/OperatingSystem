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
            if (!Exists(@"C:\NTFS")) // Если директория файловой системы не создана, то форматируем
            {
                superBlock = new SuperBlock();
                mftTable = new MFT_Table();
                Formatting();
            }

            //TODO:Если корневая директория существует, значит подгружать метаданные из JSON 
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        //TODO:Сделать создание файла
        public Stream CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public void Delete(string path)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Exists(string path)
        {
            return ProcessDirectory(path, "C:\\", 1 ,5);
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

        public Stream ReadFile(string path)
        {
            throw new NotImplementedException();
        }

        public Stream WriteFile(string path)
        {
            throw new NotImplementedException();
        }

        public void Formatting()
        {
            Array.Clear(superBlock.ClusterBitmap, 0 , superBlock.ClusterBitmap.Length); // Установка всех битов в 0
            FileSystemPath = @"C:\NTFS";
            Directory.CreateDirectory(FileSystemPath);
        }
    }
}