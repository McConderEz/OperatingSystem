using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public interface IFileSystem:IDisposable
    {
        //TODO: Переделать в асинхронку
        ICollection<string> GetEntities(string path);
        bool Exists(string path);
        void CreateFile(string fileName);
        void WriteFile(string fileName, StringBuilder data);
        void ReadFile(string path);
        void CreateDirectory(string path);
        void Formatting();
        void Delete(string path);
    }
}
