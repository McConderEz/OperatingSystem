using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model.FileSystemEnteties
{
    /// <summary>
    /// Предоставляет всю инфомрацию о файловой системе и томе
    /// </summary>
    [DataContract]
    public class SuperBlock : ISuperBlock
    {

        public readonly string Signature = "VortexOs"; //Сигнатура(Имя) файловой системы(тома)
        [DataMember]
        public string ID { get; } = Guid.NewGuid().ToString();//Уникальный идентификатор тома
        public readonly ulong SectorSize = 4096;//Размер сектора
        public readonly uint ClusterUnitSize = 4096;//Размер единицы кластера
        public readonly ulong TotalDiskSize = 8589934592;//Общий размер диска        
        public readonly ulong MFTSize;//Размер MFT(12.5% от общего размера диска) 
        public ulong ClusterBitmapOffset { get; private set; } = 8192;//Смещение битовой карты кластеров
        [DataMember]
        public byte[][] ClusterBitmap { get; private set; } // Битовая карта кластеров
        public ulong MFTOffset { get; private set; } = 0;//Смещение MFT 
        public List<MFT_Entry> MFTBackup { get; private set; }//Копия первых 16 записей MFT        


        public SuperBlock()
        {
            MFTSize = (ulong)Math.Round(TotalDiskSize * 0.125);
            MFTBackup = new List<MFT_Entry>(16);
            int totalClusterCount = 32000;
            ClusterBitmap = new byte[totalClusterCount][]; // Создание массива массивов из 2048000 кластеров, каждый из которых равен 4кб 

            for (int i = 0; i < totalClusterCount; i++)
            {
                ClusterBitmap[i] = new byte[ClusterUnitSize];
                for (int j = 0; j < ClusterUnitSize; j++)
                {
                    ClusterBitmap[i][j] = 0;
                }
            }
        }

        [JsonConstructor]
        public SuperBlock(byte[][] clusterBitmap, string id)
        {
            ID = id;
            ClusterBitmap = clusterBitmap;
            MFTSize = (ulong)Math.Round(TotalDiskSize * 0.125);
        }



        public bool IsClusterFree(int clusterIndex) => ClusterBitmap[clusterIndex][0] == 0;


        public void MarkClusterAsUsed(byte[] dataBytes, int clusterIndex)
        {
            for (int j = 0; j < dataBytes.Length; j++)
            {
                ClusterBitmap[clusterIndex][j] = dataBytes[j];
            }
        }

        public void MarkClustersAsUsedForLargeFile(MFT_Entry? mftItem, byte[] dataBytes, int dataSize, int clusterIndex)
        {
            int l = 0; // Индекс массива данных на запись 
            for (int j = clusterIndex; j < ClusterBitmap.Length; j++)
            {
                if (IsClusterFree(j))
                {
                    for (int k = 0; k < dataBytes.Length; k++)
                    {
                        if (k < 4096 && l < dataBytes.Length) // Запись байт в кластер
                        {
                            ClusterBitmap[j][k] = dataBytes[l++];
                            dataSize--;
                        }
                        else
                        {
                            // Кластер заполнен и MFT запись получает индексы на область карты кластеров, принадлежащие данному файлу
                            mftItem.Attributes.indexesOnClusterBitmap.Add(new Indexer(j));
                            break;
                        }
                    }
                }

                if (dataSize == 0) // Вся информация записана в кластеры
                {
                    break;
                }

            }
        }

        public void MarkClusterAsFree(int clusterIndex)
        {
            for (int i = 0; i < ClusterBitmap[clusterIndex].Length; i++)
            {
                ClusterBitmap[clusterIndex][i] = 0;
            }
        }

        /// <summary>
        /// Находим первый свободный кластер
        /// </summary>
        /// <returns></returns>
        public int FindFreeCluster()
        {
            for (int i = 0; i < ClusterBitmap.Length; i++)
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
