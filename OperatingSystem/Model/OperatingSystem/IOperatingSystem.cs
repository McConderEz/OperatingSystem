using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.OperatingSystem
{
    public interface IOperatingSystem
    {
        public bool Аuthorization(string login, string password);
    }
}
