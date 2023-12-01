using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    public enum AttributeFlags
    {
        ReadOnly = 0,
        NotReadOnly = 1,
        Hidden = 2,
        System = 3,
        Encrypted = 4,
    }
}
