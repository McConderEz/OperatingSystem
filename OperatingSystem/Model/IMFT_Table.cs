using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public interface IMFT_Table
    {
        void Add(string fileName,FileType fileType);
        void Delete();
        void Edit();

    }
}
