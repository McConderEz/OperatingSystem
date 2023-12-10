using OperatingSystem.Controller;
using OperatingSystem.Model.FileSystemEntities;
using OperatingSystem.Model.ProcessCommunication;
using OperatingSystem.Model.OperatingSystem;
using System.Text;
using System.Threading.Channels;
using System.Runtime.InteropServices;


OperatingSystem.Model.OperatingSystem.OperatingSystem operatingSystem = new OperatingSystem.Model.OperatingSystem.OperatingSystem();
string tempPath = "";
string tempPath2 = "";
bool exit = false;
Console.WriteLine("Загрузка системы...");
Thread.Sleep(5000);
Console.Clear();

Console.WriteLine("Авторизуйтесь в систему");
Console.WriteLine("Введите логин:");   
string login = Console.ReadLine();
Console.WriteLine("Введите пароль:");
string password = Console.ReadLine();
operatingSystem.Аuthorization(login, password);
if(operatingSystem.UserController.CurrentUser != null)
{
    Console.WriteLine("Добро пожаловать!");
}

Thread.Sleep(5000);
Console.Clear();

while (true)
{
    Console.WriteLine("Выберите действие:");
    Console.WriteLine("Q)Создание файла\nW)Запись в файл\nE)Чтение файла");
    Console.WriteLine("R)Переименование файла\nT)Копирование файла\nY)Удаление файла");
    Console.WriteLine("U)Список файлов\nI)Планировщик\nV)Генерация процессов");
    Console.WriteLine("K)Убить процесс\nA)Получить информацию о процессе\nO)Смена приоритета процесса");
    Console.WriteLine("S)Смена прав доступа файла\nG)Добавление пользователей в группу\nF)Форматирование системы\nL)Вывести список пользователей\nX)Выход из системы");
    Console.WriteLine("H)Смена пользователя\nJ)Текущий пользователь в системе");
    ConsoleKeyInfo key = Console.ReadKey();

    if(exit == true)
    {
        Console.WriteLine("Завершение работы...");
        break;
    }

    switch (key.Key)
    {
        case ConsoleKey.Q:
            Console.WriteLine("Введите полный путь к создаваемому файлу, включая имя и расширение:");
            string fullPathToCreate = Console.ReadLine();
            tempPath = @$"{fullPathToCreate}";
            operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.CreateFile(arg), tempPath);
            break;
        case ConsoleKey.W:
            Console.WriteLine("Введите полный путь к файлу, включая имя и расширение:");
            string fullPathToWrite = Console.ReadLine();
            tempPath = @$"{fullPathToWrite}";
            Console.WriteLine("Введите данные:");
            string data = Console.ReadLine();
            Console.WriteLine("Файл будем перезаписывать или дописывать данные в конец?Введите(0/1):");
            int keyAppend = int.Parse(Console.ReadLine());
            bool append = keyAppend > 0 ? true : false;
            operatingSystem.TaskPlanner.StartProcess((arg1,arg2,arg3) => operatingSystem.FileSystem.WriteFile(arg1,arg2,arg3), tempPath,new StringBuilder(data),append);
            break;
        case ConsoleKey.E:
            Console.WriteLine("Введите полный путь к файлу, включая имя и расширение:");
            string fullPathToRead = Console.ReadLine();
            tempPath = @$"{fullPathToRead}";

            Console.WriteLine("Данные с файла:");
            Console.WriteLine(operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.ReadFile(arg), tempPath));
            break;
        case ConsoleKey.R:
            Console.WriteLine("Введите полный путь к файлу, включая имя и расширение:");
            string fullPathToRename = Console.ReadLine();
            Console.WriteLine("Введите полный путь к этому файлу с новым именем, включая расширение:");
            string newFullPathToRename = Console.ReadLine();
            tempPath = $@"{fullPathToRename}";
            tempPath2 = $@"{newFullPathToRename}";

            operatingSystem.TaskPlanner.StartProcess((arg1,arg2) => operatingSystem.FileSystem.Rename(arg1,arg2), fullPathToRename, newFullPathToRename);
            break;
        case ConsoleKey.T:
            Console.WriteLine("Введите полный путь к файлу, включая имя и расширение:");
            string fullPathToOldFile = Console.ReadLine();
            Console.WriteLine("Введите полный путь для скопированного файла, включая имя и расширение:");
            string fullPathToCopyFile = Console.ReadLine();

            tempPath = $@"{fullPathToOldFile}";
            tempPath2 = $@"{fullPathToCopyFile}";
            operatingSystem.TaskPlanner.StartProcess((arg1, arg2) => operatingSystem.FileSystem.Rename(arg1, arg2), fullPathToOldFile, fullPathToCopyFile);
            break;
        case ConsoleKey.Y:
            Console.WriteLine("Введите полный путь к удаляемому файлу, включая имя и расширение:");
            string fullPathToDelete = Console.ReadLine();
            tempPath = $@"{fullPathToDelete}";
            operatingSystem.TaskPlanner.StartProcess((arg) => operatingSystem.FileSystem.Delete(arg), fullPathToDelete);
            break;
        case ConsoleKey.U:
            Console.WriteLine("Список файлов");
            var files = operatingSystem.FileSystem.GetEntities(@"D:\NTFS");
            foreach(var file in files)
            {
                Console.WriteLine(file.ToString());
            }
            break;
        case ConsoleKey.I:
            operatingSystem.TaskPlanner.GetTaskPlannerInfo();
            break;
        case ConsoleKey.O:
            Console.WriteLine("Введите PID процесса: ");
            int pid = ConvertInt();
            Console.WriteLine("Введите приоритет процесса(0-4):");
            int priority = ConvertInt();
            operatingSystem.TaskPlanner.ChangePriority((uint)pid, (uint)priority);
            break;
        case ConsoleKey.K:
            Console.WriteLine("Введите pid процесса для принудительной остановки:");
            int pidToKill = ConvertInt();
            operatingSystem.TaskPlanner.KillProcess((uint)pidToKill);
            break;
        case ConsoleKey.A:
            Console.WriteLine("Введите pid процесса:");
            int pidToGetInfo = ConvertInt();
            Console.WriteLine("Информация о процессе:");
            Console.WriteLine(operatingSystem.TaskPlanner.GetProcess((uint)pidToGetInfo));
            break;
        case ConsoleKey.S:
            Console.WriteLine("Введите полный путь к файлу, включая название и расширение:");
            string fullPathToFileCHMOD = Console.ReadLine();
            tempPath = $@"{fullPathToFileCHMOD}";
            Console.WriteLine("Введите права доступа(3 значения через пробел):");
            string input = Console.ReadLine();
            string[] values = input.Split(' ');
            if (values.Length != 3)
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите 3 числовых значения через пробел.");
            }
            else
            {
                if (int.TryParse(values[0], out int value1) && int.TryParse(values[1], out int value2) && int.TryParse(values[2], out int value3))
                {
                    operatingSystem.FileSystem.ChangeMode(tempPath, new UsersAccessFlags((AttributeFlags)value1, (AttributeFlags)value2, (AttributeFlags)value3));
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, убедитесь, что введены корректные числовые значения.");
                }
            }
            break;
        case ConsoleKey.F:
            operatingSystem.FileSystem.Formatting();
            break;
        case ConsoleKey.L:
            Console.WriteLine("Список пользователей ОС:");
            foreach (var user in operatingSystem.UserController.Users)
            {
                Console.WriteLine(user.ToString());
            }
            break;
        case ConsoleKey.X:
            exit = true;
            break;
        case ConsoleKey.V:
            Console.WriteLine("Введите число процессов:");
            int countOfProcesses = ConvertInt();
            for(int i = 0;i < countOfProcesses; i++)
            {
                operatingSystem.TaskPlanner.GenerationRandomProcess();
            }
            break;
        default:
            Console.WriteLine("Неизвестная команда!");
            break;
    }

    //Thread.Sleep(5000);
    Console.Clear();
}





static int ConvertInt()
{
    int result;

    while(!int.TryParse(Console.ReadLine(), out result))
    {
        Console.WriteLine("Неверно задано значение переменной!");
    }

    return result;
}





