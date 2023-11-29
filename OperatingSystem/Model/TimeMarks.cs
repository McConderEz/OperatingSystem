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
    /// Временные метки
    /// </summary>
    [DataContract]
    public class TimeMarks
    {
        [DataMember]
        public DateTime CreationTime { get; init; } //Дата и время создания
        [DataMember]
        public DateTime ModificationTime { get; set; } //Дата и время модификации
        [DataMember]
        public DateTime AccessTime { get; set; } //Дата и время доступа

        [JsonConstructor]
        public TimeMarks()
        {
            CreationTime = DateTime.Now;
            ModificationTime = DateTime.Now;
            AccessTime = DateTime.Now;
        }
    }
}
