using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Временные метки
    /// </summary>
    public struct TimeMarks
    {
        public DateTime CreationTime { get; init; } //Дата и время создания
        public DateTime ModificationTime { get; private set; } //Дата и время модификации
        public DateTime AccessTime { get; private set; } //Дата и время доступа

        public TimeMarks()
        {
            CreationTime = DateTime.Now;
            ModificationTime = DateTime.Now;
            AccessTime = DateTime.Now;
        }
    }
}
