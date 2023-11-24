using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public class Attribute
    {
        public MFTEntryHeader AttributeHeader { get; private set; } //Заголовок атрибута 
        public string NameHeader { get; private set; } //Заголовок имени
        public string NameData { get; private set; } //Данные имени
        public uint Length { get; } //Длина содержимого
        public uint AttributeFlags { get; private set; } // Атрибутные флаги
        public TimeMarks TimeMarks { get; private set; } // Временные метки
        public uint OwnerId { get; private set; } // Идентификатор владельца
        public uint GroupId { get; private set; } // Идентификатор группы
        public uint BlocksCount { get; private set; } // Число блоков данных
        public List<Attribute> AttributesRefs { get; private set; } // Ссылки на атрибуты, связанные с данным атрибутом
        public List<Indexer> indexesOnClusterBitmap { get; private set; } // Список индексов(начало и конец) на участки данных конкретного файла в карте св./з. кластеров

        public Attribute(MFTEntryHeader attributeHeader, string nameHeader, string nameData, uint length, uint attributeFlags = 1, uint ownerId = 1, uint groudId = 1, uint blocksCount = 0)
        {
            //TODO: Продумать атрибутные флаги, идентификаторы владельца и группы, когда будет сделана многопользовательская система
            //TODO: Сделать проверки
            AttributeHeader = attributeHeader;
            NameHeader = nameHeader;
            NameData = nameData;
            Length = length;
            AttributeFlags = attributeFlags;
            OwnerId = ownerId;
            GroupId = groudId;
            BlocksCount = blocksCount;
            indexesOnClusterBitmap = new List<Indexer>();
            AttributesRefs = new List<Attribute>();

            //TODO:Сделать изменение количества блоков кластеров, размера флагов и временных меток.
        }
    }
}
