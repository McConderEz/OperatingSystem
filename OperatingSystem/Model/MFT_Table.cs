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
    public class MFT_Table: IMFT_Table
    {
        public List<MFT_Entry> Entries { get; private set; } // Содержит все записи

        public MFT_Table()
        {
            Entries = new List<MFT_Entry>();
        }

        /// <summary>
        /// Добавление записи в таблицу при создании файла в ФС
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Add(string fileName, FileType fileType)
        {
            Entries.Add(new MFT_Entry(fileName, (uint)Entries.Count(), fileType));
        }

        /// <summary>
        /// Удаление записи из таблицы при удалении файла из ФС
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Изменение записи в таблицы при чтении/записи в файл
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Edit()
        {
            throw new NotImplementedException();
        }
    }
}
