using OperatingSystem.Model.FileSystemEnteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    

    public class Process : IProcess
    {
        //TODO: Добавить синхронизацию потоков в файлах
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

        /// <summary>
        /// Запуск процесса на исполнение 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="arg"></param>
        public void Start<T>(Action<T> method, T arg)
        {
            shouldStop = false;
            thread = new Thread(() =>
            {
                while(!shouldStop)
                {
                    method.Invoke(arg);
                    shouldStop = true;
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
        }    

        /// <summary>
        /// Запуск процесса записи в файл на исполнение
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="method"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void StartWriteMethod<T1,T2,T3>(Action<T1,T2,T3> method, T1 arg1, T2 arg2, T3 arg3)
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

        /// <summary>
        /// Запуск процесса на чтение файла
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public StringBuilder? StartReadMethod(Func<string,StringBuilder> method, string arg)
        {
            shouldStop = false;
            StringBuilder result = null;
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
        /// Запуск процесса переименования или копирования файла
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void StartCopyOrRenameMethod(Action<string, string> method, string arg1,string arg2)
        {
            shouldStop = false;
            thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    method.Invoke(arg1,arg2);
                    shouldStop = true;
                }
            });
            thread.Priority = ThreadPriority;
            thread.Start();
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
        /// Смена приоритета процесса
        /// </summary>
        /// <param name="priority"></param>
        public void ChangePriority(ThreadPriority priority)
        {
            ThreadPriority = priority;
        }
    }
}
