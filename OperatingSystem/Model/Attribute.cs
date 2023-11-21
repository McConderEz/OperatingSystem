﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public class Attribute
    {
        public string AttributeHeader { get; private set; } //Заголовок атрибута 
        public string NameHeader { get; private set; } //Заголовок имени
        public string NameData { get; private set; } //Данные имени
        public uint Length { get; } //Длина содержимого
        public uint AttributeFlags { get; private set; } // Атрибутные флаги
        public TimeMarks TimeMarks { get; private set; } // Временные метки
        public uint OwnerId { get; private set; } // Идентификатор владельца
        public uint GroupId { get; private set; } // Идентификатор группы
        public uint BlocksCount { get; private set; } // Число блоков данных
        public List<Attribute> AttributesRefs { get; private set; } // Ссылки на атрибуты, связанные с данным атрибутом


    }
}
