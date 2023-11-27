﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public struct MFTEntryHeader
    {
        public string Signature { get; private set; }
        public uint LogSequenceNumber { get; private set; } //TODO:Сделать после реализации Журналирования
        public uint SequenceNumber { get; private set; }
        public FileType Flag { get; private set; }

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
