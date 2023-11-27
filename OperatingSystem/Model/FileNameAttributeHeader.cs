using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public class FileNameAttributeHeader
    {
        public ushort ParentDirectoryIndex { get; set; }
        public ulong AllocatedSize { get; set; }
        public ulong DataSize { get; set; }
        public uint Flags { get; set; }
        public uint EaSize { get; set; }
        public byte NameLength { get; set; }
        public byte Namespace { get; set; }
        public string Name { get; set; }
    }
}
