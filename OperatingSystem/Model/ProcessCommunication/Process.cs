using OperatingSystem.Model.FileSystemEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    

    public class Process : IProcess
    {
        public Func<string,StringBuilder> ReadFileDelegate { get; set; }
        public Action<string> InputMethod { get; set; }
        public Action<string, StringBuilder, bool> WriteMethod { get; set; }
        public Action<string, string> CopyAndRenameDelegate { get; set; }
    

        public uint PID { get; init; } //Уникальный идентификатор процесса
        public string ProcessName { get; init; } //Имя процесса
        public DateTime StartTime { get; init; } //Время запуска процесса
        public ThreadPriority ThreadPriority // Приоритет процесса
        {
            get => thread.Priority;
            set 
            { 
                if(thread != null && thread.ThreadState == ThreadState.Running)
                {
                    thread.Priority = value;
                } 
            }
        }

        public ThreadState ThreadState { get => thread.ThreadState; } //Состояние процесса


        public static List<uint> pids;

        private bool shouldStop;
        private Thread thread;
       

        public Process()
        {
            Random rnd = new Random();
            pids = new List<uint>() { 0 };
            PID = PIDGenerator();
            ProcessName = Guid.NewGuid().ToString(); //Потом поменять на действие + файл
            StartTime = DateTime.Now;
            pids.Add(PID);           
        }

        /// <summary>
        /// Немедленное завершение процесса
        /// </summary>
        public void Kill()
        {
            shouldStop = true;
        }

        public void Start<T>(Action<T> method, T arg)
        {
            shouldStop = false;
            thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    method.Invoke(arg);
                    shouldStop = true;                   
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
        }

        public void StartMethod<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            shouldStop = false;
            thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    method.Invoke(arg1, arg2, arg3);
                    shouldStop = true;
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
        }

        public void StartMethod<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2)
        {
            shouldStop = false;
            thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    method.Invoke(arg1, arg2);
                    shouldStop = true;
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
        }

        public TResult StartMethod<TResult, TArg>(Func<TArg, TResult> method, TArg arg)
        {
            shouldStop = false;
            TResult result = default;
            thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    result = method.Invoke(arg);
                    shouldStop = true;
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
            thread.Join();

            return result;
        }

        

        /// <summary>
        /// Генерация уникального идентификатора процесса
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Запуск имитации работы процесса
        /// </summary>
        public void StartRandomProcess()
        {
            shouldStop = false;
            thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    for (var i = 0; i < new Random().Next(1000, 1000000); i++)
                    {
                        Thread.Sleep(1);
                    }
                    shouldStop = true;
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
        }

        /// <summary>
        /// Смена приоритета процесса
        /// </summary>
        /// <param name="priority"></param>
        public void ChangePriority(uint priority)
        {
            if (priority <= 4) 
            {
                ThreadPriority = (ThreadPriority)priority;
            }
            else
            {
                ThreadPriority = ThreadPriority.Highest;
            }
        }

        public override string ToString()
        {
            return $"{PID}\t{ProcessName}\t{ThreadPriority}\t{ThreadState}\t{StartTime}";
        }
    }
}
