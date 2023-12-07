using OperatingSystem.Controller;
using OperatingSystem.Model.FileSystemEntities;
using OperatingSystem.Model.ProcessCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model.OperatingSystem
{
    public class OperatingSystem: IOperatingSystem
    {
        public FileSystem FileSystem;
        public TaskPlanner TaskPlanner;
        public UserController UserController;

        public OperatingSystem()
        {
            FileSystem = FileSystem.Instance;
            TaskPlanner = TaskPlanner.Instance;
        }

        public bool Аuthorization(string login, string password)
        {
            UserController = new UserController(login, password);

            if(UserController.CurrentUser != null)
            {
                FileSystem.SetUserController(UserController);
                return true;
            }

            return false;
        }
    }
}
