using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    /// <summary>
    /// Структура определяющие начальный и конечный индекс для участка данных в карте кластеров
    /// </summary>
    [DataContract]
    public struct Indexer
    {
        [DataMember]
        public int Index { get; set; }


        public Indexer()
        {
            Index = -1;
        }

        [JsonConstructor]
        public Indexer(int index)
        {
            Index = index;
        }
    }
}
