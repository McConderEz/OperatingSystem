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
            if(login.Equals("root") && password.Equals("root"))
            {
                EnterAsRoot();
            }

            UserController = new UserController(login, password);

            if(UserController.CurrentUser != null)
            {
                FileSystem.SetUserController(UserController);
                return true;
            }

            return false;
        }

        private bool EnterAsRoot()
        {
            UserController = new UserController("root", "root");
            if(UserController != null)
            {
                FileSystem.SetUserController(UserController);
                return true;
            }
            return false;
        }

        public void AddNewUser(string login, string password)
        {
            UserController = new UserController();
            UserController.AddNewUser(login, password);
        } 

        public bool EnterAsGuest()
        {
            UserController = new UserController("guest","guest");
            if (UserController.CurrentUser != null)
            {
                FileSystem.SetUserController(UserController);
                return true;
            }
            return false;
        }
    }
}
