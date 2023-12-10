using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.Memory
{
    public class VirtualMemory
    {
        private byte[][] memory;
        private int pageSize;

        public VirtualMemory(int pageCount, int pageSize)
        {
            this.pageSize = pageSize;
            memory = new byte[pageSize][];

            for(int i = 0; i < pageSize; i++)
            {
                memory[i] = new byte[pageSize];
            }
        }

        public byte[] ReadPage(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber >= memory.Length)
            {
                throw new ArgumentOutOfRangeException("Неверный номер страницы", nameof(pageNumber));
            }

            return memory[pageNumber];
        }

        public void WritePage(int pageNumber, byte[] data)
        {
            if(pageNumber < 0 || pageNumber >= memory.Length)
            {
                throw new ArgumentOutOfRangeException("Неверный номер страницы", nameof(pageNumber));
            }

            if(data.Length > pageSize)
            {
                throw new ArgumentException("Размер данных превышает размер страницы", nameof(data.Length));
            }

            Array.Copy(data, memory[pageNumber], data.Length);
        }
    }
}
