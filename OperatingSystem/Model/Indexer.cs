using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Структура определяющие начальный и конечный индекс для участка данных в карте кластеров
    /// </summary>
    [DataContract]
    public struct Indexer
    {
        [DataMember]
        public int index;

        [JsonConstructor]
        public Indexer(int index = -1)
        {
            this.index = index;
        }
    }
}
