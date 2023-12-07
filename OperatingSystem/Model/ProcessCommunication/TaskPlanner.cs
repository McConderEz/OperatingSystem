using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    public class TaskPlanner
    {
        //TODO: Добавить отслеживание процессов в реальном времени(когда будет готов интерфейс)
        private static TaskPlanner instance;
        private static readonly object lockObject = new object();
        public List<Process> Processes { get; set; }

        private TaskPlanner()
        {
            Processes = new List<Process>();
        }

        public static TaskPlanner Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(lockObject)
                    {
                        if(instance == null)
                        {
                            instance = new TaskPlanner();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Удаление процесса по уникальному идентификатору процесса
        /// </summary>
        /// <param name="PID"></param>
        public void KillProcess(uint PID)
        {
            var process = Processes.SingleOrDefault(p => p.PID.Equals(PID));

            if(process != null)
            {
                process.Kill();
                Processes.Remove(process);
            }
        }

        /// <summary>
        /// Вернуть процесс по уникальному идентификатору 
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public Process GetProcess(uint PID)
        {
            var process = Processes.SingleOrDefault(p => p.PID.Equals(PID));

            if(process != null)
            {
                return process;
            }

            return null;
        }

        /// <summary>
        /// Изменение приоритета процесса по уникальному идентификатору
        /// </summary>
        /// <param name="PID"></param>
        /// <param name="priority"></param>
        public void ChangePriority(uint PID, uint priority)
        {
            var process = Processes.SingleOrDefault(p => p.PID.Equals(PID));

            if(process != null)
            {
                process.ChangePriority(priority);
            }
        }

        /// <summary>
        /// Запуск нового процесса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="arg"></param>
        public void StartProcess<T>(Action<T> method, T arg)
        {
            Process process = new Process();
            Processes.Add(process);
            process.Start(method, arg);
        }

        /// <summary>
        /// Генерация рандомного процесса
        /// </summary>
        public void GenerationRandomProcess()
        {
            Process process = new Process();
            Processes.Add(process);
            process.StartRandomProcess();
        }

        /// <summary>
        /// Запуск нового процесса
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="method"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void StartProcess<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            Process process = new Process();
            Processes.Add(process);
            process.StartMethod(method, arg1, arg2, arg3);
        }

        /// <summary>
        /// Запуск нового процесса
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="method"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public TResult StartProcess<TResult, TArg>(Func<TArg, TResult> method, TArg arg)
        {
            Process process = new Process();
            Processes.Add(process);
            return process.StartMethod(method, arg);
        }

    }
}
