using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEntities
{
    public class JournalEntry
    {
        public DateTime Timestamp { get; set; }
        public string OperationType { get; set; }
        public string Description { get; set; }
        public string LogSequenceNumber { get; set; }
        public override string ToString()
        {
            return $"[{Timestamp}] ({LogSequenceNumber}) {OperationType} - {Description}";
        }
    }
}
