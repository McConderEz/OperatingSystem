using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    [DataContract]
    public struct MFTEntryHeader
    {
        [DataMember]
        public string Signature { get; private set; }
        [DataMember]
        public uint LogSequenceNumber { get; private set; } //TODO:Сделать после реализации Журналирования
        [DataMember]
        public uint SequenceNumber { get; private set; }
        [DataMember]
        public FileType Flag { get; private set; }

        [JsonConstructor]
        public MFTEntryHeader(string signature, uint sequenceNumber, FileType flag = FileType.File)
        {
            if (string.IsNullOrWhiteSpace(signature))
            {
                throw new ArgumentNullException("Сигнатура не может быть пустой",nameof(signature));
            }            
            if(sequenceNumber < 0)
            {
                throw new ArgumentException("Номер последовательности не может быть меньше нуля!", nameof(sequenceNumber));
            }

            Signature = signature;
            SequenceNumber = sequenceNumber;
            Flag = flag;

        }
    }
}
