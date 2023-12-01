using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    [DataContract]
    public class Attribute
    {
        [DataMember]
        public MFTEntryHeader AttributeHeader { get; private set; } //Заголовок атрибута 
        [DataMember]
        public string NameData { get; private set; } //Данные имени
        [DataMember]
        public string FullPath { get; private set; } //Полный путь файла
        [DataMember]
        public string ParentsDirectory { get; private set; } //Родительская директория
        [DataMember]
        public FileType FileType { get; init; } //Тип записи:файл, каталог...
        [DataMember]
        public uint Length { get; private set; } //Длина содержимого
        [DataMember]
        public AttributeFlags AttributeFlags { get; private set; } // Атрибутные флаги
        [DataMember]
        public TimeMarks TimeMarks { get; private set; } // Временные метки
        [DataMember]
        public uint OwnerId { get; private set; } // Идентификатор владельца
        [DataMember]
        public uint GroupId { get; private set; } // Идентификатор группы
        [DataMember]
        public uint BlocksCount { get; private set; } // Число блоков данных
        [DataMember]
        public List<Attribute> AttributesRefs { get; private set; } // Ссылки на атрибуты, связанные с данным атрибутом
        [DataMember]
        public List<Indexer> indexesOnClusterBitmap { get; private set; } // Список индексов(начало и конец) на участки данных конкретного файла в карте св./з. кластеров

        public Attribute(MFTEntryHeader attributeHeader, string nameData, string fullPath, uint length, FileType fileType = FileType.File,
            AttributeFlags attributeFlags = AttributeFlags.NotReadOnly, uint ownerId = 1, uint groudId = 1, uint blocksCount = 0)
        {
            //TODO: Продумать атрибутные флаги, идентификаторы владельца и группы, когда будет сделана многопользовательская система

            if (string.IsNullOrWhiteSpace(nameData))
            {
                throw new ArgumentNullException("Данные имени не могут быть пустыми", nameof(nameData));
            }

            if (string.IsNullOrWhiteSpace(fullPath))
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



        [JsonConstructor]
        public Attribute(MFTEntryHeader attributeHeader, string nameData, string fullPath, string parentsDirectory, FileType fileType, uint length, AttributeFlags attributeFlags, TimeMarks timeMarks,
    uint ownerId, uint groupId, uint blocksCount, List<Attribute> attributesRefs, List<Indexer> indexesOnClusterBitmap)
        {
            AttributeHeader = attributeHeader;
            NameData = nameData;
            FullPath = fullPath;
            FileType = fileType;
            Length = length;
            AttributeFlags = attributeFlags;
            OwnerId = ownerId;
            GroupId = groupId;
            BlocksCount = blocksCount;
            AttributesRefs = attributesRefs;
            TimeMarks = timeMarks;
            ParentsDirectory = parentsDirectory;
            this.indexesOnClusterBitmap = indexesOnClusterBitmap;
        }

        private static string GetParentsDir(string fullPath)
        {
            char[] separators = { '\\', '/', '\\' };
            int lastIndex = fullPath.LastIndexOfAny(separators);
            if (lastIndex >= 0)
            {
                string result = fullPath.Substring(0, lastIndex);
                return result;
            }

            return "";
        }

        public void Edit(FileInfo fileInfo,uint length, uint blocksCount)
        {
            FullPath = fileInfo.FullName;
            NameData = fileInfo.Name;
            ParentsDirectory = fileInfo.DirectoryName;
            TimeMarks.AccessTime = DateTime.Now;
            TimeMarks.ModificationTime = DateTime.Now;
            BlocksCount = blocksCount;
            Length = length;
        }
    }
}
