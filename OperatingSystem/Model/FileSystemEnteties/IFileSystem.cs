using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    public interface IFileSystem : IDisposable
    {
        ICollection<string> GetEntities(string path);
        bool Exists(string path);
        void CreateFile(string fileName);
        void WriteFile(string fileName, StringBuilder data, bool append);
        StringBuilder ReadFile(string path);
        void CreateDirectory(string path);
        void Formatting();
        void Delete(string path);
        void CopyTo(string oldFilePath, string newFilePath);
        void MoveTo(string path);
        void Rename(string path, string newName);
    }
}
