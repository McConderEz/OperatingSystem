using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEntities
{
    public class Journal
    {
        private List<JournalEntry> entries;

        public Journal()
        {
            entries = new List<JournalEntry>();
        }

        /// <summary>
        /// Добавление записи в журнал
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(JournalEntry entry)
        {
            entries.Add(entry);
        }

        /// <summary>
        /// Логгирование
        /// </summary>
        public async Task Logging()
        {
            using(FileStream fs = new FileStream("Log.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (JournalEntry entry in entries)
                    {
                        sw.WriteLine(entry.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Получение списка записей журнала
        /// </summary>
        /// <returns></returns>
        public List<JournalEntry> GetEntries()
        {
            return entries;
        }

    }
}
