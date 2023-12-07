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
        Read = 0x1,
        Write = 0x2,
        Execute = 0x4,
        Delete = 0x8,
        Modify = Read & Write & Execute & Delete,
        ChangeMode = 0x16,
        FullControl = Modify & ChangeMode
    }
}
