using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    public class TaskPlanner
    {
        //TODO: Добавить функции добавления процесса, удаления процесса, смена приоритета, вывод информации о конкретном процессе,
        //генерацию случайного процесса
        //TODO: Добавить отслеживание процессов в реальном времени
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

    }
}
