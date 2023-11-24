using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    /// <summary>
    /// Предоставляет всю инфомрацию о файловой системе и томе
    /// </summary>
    public class SuperBlock: ISuperBlock
    {        
        public readonly string Signature = "VortexOs"; //Сигнатура(Имя) файловой системы(тома)
        public readonly string ID = Guid.NewGuid().ToString();//Уникальный идентификатор тома
        public readonly ulong SectorSize = 4096;//Размер сектора
        public readonly uint ClusterUnitSize = 4096;//Размер единицы кластера
        public readonly ulong TotalDiskSize = 8589934592;//Общий размер диска
        public readonly ulong MFTSize;//Размер MFT(12.5% от общего размера диска) 
        public ulong ClusterBitmapOffset { get; private set; } = 8192;//Смещение битовой карты кластеров
        public byte[][] ClusterBitmap { get; private set; } // Битовая карта кластеров
        public ulong MFTOffset { get; private set; } = 0;//Смещение MFT 
        public List<MFT_Entry> MFTBackup { get; private set; }//Копия первых 16 записей MFT        

        public SuperBlock()
        {
            MFTSize = (ulong)Math.Round(TotalDiskSize * 0.125);
            MFTBackup = new List<MFT_Entry>(16);
            int totalClusterCount = 2048;
            ClusterBitmap = new byte[totalClusterCount][]; // Создание массива массивов из 2048000 кластеров, каждый из которых равен 4кб 

            for(int i = 0;i < totalClusterCount; i++)
            {
                ClusterBitmap[i] = new byte[ClusterUnitSize];
                for (int j = 0; j < ClusterUnitSize; j++)
                {
                    ClusterBitmap[i][j] = 0;
                }
            }
        }



        public bool IsClusterFree(int clusterIndex) => ClusterBitmap[clusterIndex][0] == 0;
        

        public void MarkClusterAsUsed(int clusterIndex)
        {
            throw new NotImplementedException();
        }

        public void MarkClusterAsFree(int clusterIndex)
        {
            throw new NotImplementedException();
        }

        public int FindFreeCluser()
        {
            for(int i = 0;i < ClusterBitmap.Length;i++)
            {
                if (ClusterBitmap[i][0] == 0)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
