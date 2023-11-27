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
        public void Add(string fileName,string fullPath ,FileType fileType)
        {
            Entries.Add(new MFT_Entry(fileName,fullPath ,(uint)Entries.Count(), fileType));
        }

        /// <summary>
        /// Удаление записи из таблицы при удалении файла из ФС
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(string fullPath, SuperBlock superBlock)
        {
            var entry = Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));
            if (entry != null)
            {
                for(int i = 0;i < entry.Attributes.indexesOnClusterBitmap.Count;i++) //Освобождение кластеров, принадлежащих файлу
                {
                    superBlock.MarkClusterAsFree(entry.Attributes.indexesOnClusterBitmap[i].index);
                }
                Entries.Remove(entry); //Удаляем Entry из MFT
            }
        }

        /// <summary>
        /// Изменение записи в таблицы при чтении/записи в файл
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Edit(string fullPath, uint length)
        {
            var entry = Entries.SingleOrDefault(x => x.Attributes.FullPath.Equals(fullPath));
            if(entry != null)
            {
                entry.Attributes.Edit(length, (uint)entry.Attributes.indexesOnClusterBitmap.Count);
            }
        }
    }
}
