using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    

    public class Process : IProcess
    {
        

        public uint PID { get; init; } //Уникальный идентификатор процесса
        public string ProcessName { get; init; } //Имя процесса
        public DateTime StartTime { get; init; } //Время запуска процесса
        public ThreadPriority ThreadPriority { get; set; } //Приоритет процесса 
        public ThreadState ThreadState { get; set; } //Состояние процесса

        public static List<uint> pids;

       

        public Process()
        {
            Random rnd = new Random();
            pids = new List<uint>() { 0 };
            PID = PIDGenerator();
            ProcessName = Guid.NewGuid().ToString(); //Потом поменять на действие + файл
            StartTime = DateTime.Now;
            ThreadPriority = ThreadPriority.Normal;
            ThreadState = ThreadState.Running;
            pids.Add(PID);
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            
        }

        private uint PIDGenerator()
        {
            uint PID = 0;
            Random rnd = new Random();
            while(pids.All(x => x == PID))
            {
                PID = (uint)rnd.Next(1, 100000);
            }

            return PID;
        }
    }
}
