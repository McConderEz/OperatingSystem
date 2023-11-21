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
        Stream CreateFile(string path);
        Stream WriteFile(string path);
        Stream ReadFile(string path);
        void CreateDirectory(string path);
        void Formatting();
        void Delete(string path);
    }
}
