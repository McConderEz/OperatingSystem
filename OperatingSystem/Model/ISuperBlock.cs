using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public interface ISuperBlock
    {
        bool IsClusterFree(int clusterIndex);
        //1)Пробегать по ClusterBitmap,если первый элемент одного кластера(ClusterBitmap[i][0]) не равен 0, то
        //кластер занят и мы не можем его использовать, даже если кластер заполнен не на все 4кб. Ищем свободный кластер(где все элементы кластера = 0)
        //2)Далее посчитать размер данных и создать bin файл, куда записать данные из кластеров по индексам и сохранить всю информацию о файле(метаданные) в MFT->MFT-Entry->Attribute
        void MarkClusterAsUsed(int clusterIndex);
        void MarkClusterAsFree(int clusterIndex);

        int FindFreeCluser();

    }
}
