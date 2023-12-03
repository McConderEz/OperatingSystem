using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.ProcessCommunication
{
    public class TaskPlanner
    {
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
