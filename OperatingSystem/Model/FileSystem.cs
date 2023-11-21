namespace OperatingSystem.Model
{
    /// <summary>
    /// Файловая система NTFS
    /// </summary>
    public class FileSystem : IFileSystem
    {
        public static string FileSystemPath { get; private set; } // Путь файловой системы

        
        public FileSystem()
        {
            if (!Exists(@"C:\NTFS")) // Если директория файловой системы не создана, то форматируем
            {
                Formatting();
            }
            //TODO:Инициализовать суперблок и файловую таблицу
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

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
            FileSystemPath = @"C:\NTFS";
            Directory.CreateDirectory(FileSystemPath);
        }
    }
}