using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    //TODO:Дополнить класс
    public class SuperBlock
    {        
        public readonly string Signature = "VortexOs"; //Сигнатура(Имя) файловой системы(тома)
        public readonly string ID = Guid.NewGuid().ToString();//Уникальный идентификатор тома
        public readonly ulong SectorSize = 4096;//Размер сектора
        public readonly uint ClusterUnitSize = 4096;//Размер единицы кластера
        public readonly ulong TotalDiskSize = 53687091200;//Общий размер диска
        public readonly ulong MFTSize;//Размер MFT(12.5% от общего размера диска) 
        public ulong ClusterBitmapOffset { get; private set; } = 8192;//Смещение битовой карты кластеров
        public ulong MFTOffset { get; private set; } = 0;//Смещение MFT 
        public List<MFT_Entry> MFTBackup { get; private set; }//Копия первых 16 записей MFT        

        public SuperBlock()
        {
            MFTSize = (ulong)Math.Round(TotalDiskSize * 0.125);
            MFTBackup = new List<MFT_Entry>(16);
        }
    }
}
