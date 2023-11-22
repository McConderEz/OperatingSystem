using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Предоставляет хранение записей, которые содержат метаданные о файлах и ссылки на них
    /// </summary>
    public class MFT_Table
    {
        public List<MFT_Entry> Entries { get; private set; } // Содержит все записи

        public MFT_Table()
        {
            Entries = new List<MFT_Entry>();
        }

    }
}
