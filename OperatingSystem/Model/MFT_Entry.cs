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

        public MFT_Entry(string fileName, uint sequenceNumber, FileType fileType)
        {
            Header = new MFTEntryHeader(fileName,sequenceNumber,fileType);
            Attributes = new Attribute();//TODO:Сделать заполнение
        }
    }
}
