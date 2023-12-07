using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEntities
{
    public interface IMFT_Table
    {
        void Add(string fileName, string fullPath, FileType fileType, string logSequenceNumber, uint ownerId, List<uint> groupdId);
        void Delete(string fullPath, SuperBlock superBlock);
        void Edit(string fullPath, uint length, FileInfo fileInfo);

    }
}
