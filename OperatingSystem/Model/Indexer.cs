using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Структура определяющие начальный и конечный индекс для участка данных в карте кластеров
    /// </summary>
    public struct Indexer
    {
        public int indexStart;
        public int indexEnd;

        public Indexer()
        {
            indexStart = 0;
            indexEnd = 0;
        }
    }
}
