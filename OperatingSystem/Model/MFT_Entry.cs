using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public class MFT_Entry
    {
        public MFTEntryHeader Header { get; private set; } // Заголовок записи
        public Attribute Attributes { get; private set; } // Атрибуты записи и файла

        public readonly string ID = Guid.NewGuid().ToString(); // Уникальный идентификатор записи

        public MFT_Entry(string fileName,string fullPath ,uint sequenceNumber, FileType fileType)
        {
            Header = new MFTEntryHeader(fileName,sequenceNumber,fileType);
            Attributes = new Attribute(Header,fileName,fullPath , (uint)new FileInfo(fullPath).Length, fileType, AttributeFlags.NotReadOnly, 1, 1, 0);//TODO:Сделать заполнение            
        }
    }
}
