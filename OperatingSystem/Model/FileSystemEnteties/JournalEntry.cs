using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    public class JournalEntry
    {
        public DateTime Timestamp { get; set; }
        public string OperationType { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"[{Timestamp}] {OperationType} - {Description}";
        }
    }
}
