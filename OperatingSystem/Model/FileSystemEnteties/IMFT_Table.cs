using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    public interface IMFT_Table
    {
        void Add(string fileName, string fullPath, FileType fileType);
        void Delete(string fullPath, SuperBlock superBlock);
        void Edit(string fullPath, uint length, FileInfo fileInfo);

    }
}
