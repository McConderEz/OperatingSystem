using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEntities
{
    [Flags]
    public enum AttributeFlags
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        Delete = 8,
        Modify = 15,
        ChangeMode = 16,
        FullControl = 31
    }
}
