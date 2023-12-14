using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{

    public enum ProcessState
    {
        Running,
        Waiting,
        Completed
    }

    public enum ProcessPriority
    {
        Low,
        Medium,
        High
    }

    public class MyProcess
    {
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public int ExecutionTime { get; set; }
        public int RemainingTime { get; set; }
        public ProcessPriority Priority { get; set; }
        public ProcessState State { get; set; }

        public MyProcess(int pid, string processName, int executionTime, ProcessPriority priority)
        {
            PID = pid;
            ProcessName = processName;
            ExecutionTime = executionTime;
            RemainingTime = executionTime;
            Priority = priority;
            State = ProcessState.Waiting;
        }
    }

    public class Scheduler
    {
        private List<MyProcess> processes;
        private int timeQuantum;

        public Scheduler(int timeQuantum)
        {
            this.timeQuantum = timeQuantum;
            processes = new List<MyProcess>();
        }

        public void AddProcess(MyProcess process)
        {
            processes.Add(process);
        }

        public void Run()
        {
            Console.WriteLine("Starting Scheduler...");

            while (processes.Count > 0)
            {
                MyProcess currentProcess = GetNextProcess();
                if (currentProcess != null)
                {
                    RunProcess(currentProcess);
                    if (currentProcess.RemainingTime > 0)
                    {
                        // Если процесс не завершился, добавляем его в конец списка процессов
                        processes.Add(currentProcess);
                    }
                    else
                    {
                        Console.WriteLine($"Process {currentProcess.PID} ({currentProcess.ProcessName}) completed.");
                        processes.Remove(currentProcess);
                    }
                }

                PrintProcessTable();
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine("Scheduler finished.");
        }

        private MyProcess GetNextProcess()
        {
            if (processes.Count == 0)
            {
                return null;
            }

            // Сортируем процессы по абсолютному приоритету и оставшемуся времени выполнения
            processes.Sort((p1, p2) => {
                int priorityCompare = p2.Priority.CompareTo(p1.Priority);
                if (priorityCompare != 0)
                {
                    return priorityCompare;
                }
                else
                {
                    return p2.RemainingTime.CompareTo(p1.RemainingTime);
                }
            });

            return processes[0];
        }

        private void RunProcess(MyProcess process)
        {
            Console.WriteLine($"Running process {process.PID} ({process.ProcessName}) with priority {process.Priority}...");

            process.State = ProcessState.Running;

            // Выполняем процесс в квантах времени
            for (int i = 0; i < timeQuantum; i++)
            {
                Thread.Sleep(100); // Имитируем выполнение процесса
                process.RemainingTime--;
                if (process.RemainingTime <= 0)
                {
                    break;
                }
            }

            process.State = ProcessState.Completed;
        }

        private void PrintProcessTable()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|  PID  |    Process Name    |  Execution Time  |  Remaining Time  |  Priority  |     State      |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

            foreach (var process in processes)
            {
                Console.WriteLine($"|  {process.PID,-5} |  {process.ProcessName,-18} |  {process.ExecutionTime,-15} |  {process.RemainingTime,-15} |  {process.Priority,-10} |  {process.State,-15} |");
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
    }


}
