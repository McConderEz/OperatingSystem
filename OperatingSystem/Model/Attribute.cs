using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public class Attribute
    {
        public MFTEntryHeader AttributeHeader { get; private set; } //Заголовок атрибута 
        public string NameData { get; private set; } //Данные имени
        public string FullPath { get; private set; } //Полный путь файла
        public string ParentsDirectory { get; private set; } //Родительская директория
        public FileType FileType { get; init; } //Тип записи:файл, каталог...
        public uint Length { get; private set; } //Длина содержимого
        public AttributeFlags AttributeFlags { get; private set; } // Атрибутные флаги
        public TimeMarks TimeMarks { get; private set; } // Временные метки
        public uint OwnerId { get; private set; } // Идентификатор владельца
        public uint GroupId { get; private set; } // Идентификатор группы
        public uint BlocksCount { get; private set; } // Число блоков данных
        public List<Attribute> AttributesRefs { get; private set; } // Ссылки на атрибуты, связанные с данным атрибутом
        public List<Indexer> indexesOnClusterBitmap { get; private set; } // Список индексов(начало и конец) на участки данных конкретного файла в карте св./з. кластеров

        public Attribute(MFTEntryHeader attributeHeader, string nameData, string fullPath ,uint length, FileType fileType = FileType.File ,
            AttributeFlags attributeFlags = AttributeFlags.NotReadOnly, uint ownerId = 1, uint groudId = 1, uint blocksCount = 0)
        {
            //TODO: Продумать атрибутные флаги, идентификаторы владельца и группы, когда будет сделана многопользовательская система

            if (string.IsNullOrWhiteSpace(nameData))
            {
                throw new ArgumentNullException("Данные имени не могут быть пустыми", nameof(nameData));
            }
            
            if(string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentNullException("Полный путь не может быть пустым", nameof(fullPath));
            }
            

            AttributeHeader = attributeHeader;
            NameData = nameData;
            FullPath = fullPath;
            FileType = fileType;
            Length = length;
            AttributeFlags = attributeFlags;
            OwnerId = ownerId;
            GroupId = groudId;
            BlocksCount = blocksCount;
            indexesOnClusterBitmap = new List<Indexer>();
            AttributesRefs = new List<Attribute>();
            TimeMarks = new TimeMarks();
            ParentsDirectory = GetParentsDir(fullPath);
            //TODO:Сделать изменение количества блоков кластеров, размера флагов и временных меток.
        }

        private static string GetParentsDir(string fullPath)
        {
            char[] separators = { '\\', '/', '\\' };
            int lastIndex = fullPath.LastIndexOfAny(separators);
            if(lastIndex >= 0)
            {
                string result = fullPath.Substring(0,lastIndex);
                return result;
            }

            return "";
        }

        public void Edit(uint length,uint blocksCount)
        {            
            TimeMarks.AccessTime = DateTime.Now;
            TimeMarks.ModificationTime = DateTime.Now;
            BlocksCount = blocksCount;
            Length = length;
        }
    }
}
