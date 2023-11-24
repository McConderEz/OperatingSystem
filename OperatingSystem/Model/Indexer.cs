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
        public int index;

        public Indexer(int index = -1)
        {
            this.index = index;
        }
    }
}
