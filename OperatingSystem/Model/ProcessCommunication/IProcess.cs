using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OperatingSystem.Model.ProcessCommunication.Process;

namespace OperatingSystem.Model.ProcessCommunication
{
    public interface IProcess
    {
        void Kill();
        void Start<T>(Action<T> method, T arg);
        void ChangePriority(uint priority);

    }
}
