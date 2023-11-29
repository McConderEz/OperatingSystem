using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    [DataContract]
    public class MFT_Entry
    {
        [DataMember]
        public MFTEntryHeader Header { get; private set; } // Заголовок записи
        [DataMember]
        public Attribute Attributes { get; private set; } // Атрибуты записи и файла

        [DataMember]
        public string ID { get; } = Guid.NewGuid().ToString(); // Уникальный идентификатор записи

        public MFT_Entry(string fileName,string fullPath ,uint sequenceNumber, FileType fileType)
        {
            Header = new MFTEntryHeader(fileName,sequenceNumber,fileType);
            Attributes = new Attribute(Header,fileName,fullPath , (uint)new FileInfo(fullPath).Length, fileType, AttributeFlags.NotReadOnly, 1, 1, 0);//TODO:Сделать заполнение            
        }

        [JsonConstructor]
        public MFT_Entry(MFTEntryHeader header, Attribute attributes, string id)
        {
            Header= header;
            Attributes= attributes;
            ID= id;
        }
    }
}
