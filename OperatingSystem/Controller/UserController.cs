using OperatingSystem.Model.MultiUserProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Controller
{
    public class UserController : ControllerSaveBase
    {
        private const string USERS_FILE_NAME = "users.json";
        public List<User> Users { get; }
        public User CurrentUser { get; set; }
        public bool IsNewUser { get; } = false;
        private ControllerCryptography ControllerCryptograph { get; set; }

        public UserController(string login, string password)
        {
            ControllerCryptograph = new ControllerCryptography();

            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login), "Имя пользователя не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Пароль не может быть пустым");
            }

            Users = GetUsersDataAsync().Result;

            var root = Users.SingleOrDefault(u => u.Login == "root" && ControllerCryptograph.ValidatePassword("root", u.HashPassword));

            if(root == null)
            {
                Users.Add(new User(0, new List<uint> { 0 }, login, ControllerCryptograph.GenerateHash(password), DateTime.Now, AccountType.Administrator));
                SaveAsync();
            }

            if (login.Equals("root") && password.Equals("root"))
            {
                root = Users.SingleOrDefault(u => u.Login == "root" && ControllerCryptograph.ValidatePassword("root", u.HashPassword));
                CurrentUser = root;
                
            }
            else if (login.Equals("guest") && password.Equals("guest"))
            {
                CurrentUser = new User(0, new List<uint> { 0 }, login, ControllerCryptograph.GenerateHash(password), DateTime.Now, AccountType.Guest);
            }
            else
            {
                CurrentUser = Users.SingleOrDefault(u => u.Login == login && ControllerCryptograph.ValidatePassword(password, u.HashPassword));
            }

           
            if (CurrentUser == null)
            {
                //var user = Users.OrderByDescending(x => x.Id).FirstOrDefault();
                //uint id;
                //if(user == null)
                //{
                //    id = 1;
                //}
                //else
                //{
                //    id = user.Id + 1;
                //}
                //var hashPassword = ControllerCryptograph.GenerateHash(password);
                //CurrentUser = new User(id,new List<uint>() { 0 },login, hashPassword, DateTime.Now);
                //Users.Add(CurrentUser);
                //IsNewUser = true;
                //SaveAsync();
                Console.WriteLine("Пользователь с такими данными не найден");
            }

        }

        public List<uint> GetAllGroupsOfUser(string login)
        {
            var user = Users.SingleOrDefault(u => u.Login.Equals(login));

            if(user != null)
            {
                return user.IdGroup;
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
                return null;
            }
        }

        public UserController()
        {
            Users = GetUsersDataAsync().Result;
            ControllerCryptograph = new ControllerCryptography();
        }

        public void AddNewUser(string login,string password)
        {
            try
            {
                if (login.Equals("root"))
                {
                    Console.WriteLine("Данное имя зарезервировано");
                    return;
                }

                var flag = Users.Exists(u => u.Login.Equals(login));

                if (string.IsNullOrWhiteSpace(login))
                {
                    throw new ArgumentNullException($"{login} is not a valid login", nameof(login));
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentNullException($"{password} is not a valid login", nameof(password));
                }

                if (flag)
                {
                    Console.WriteLine("Аккаунт с таким именем уже существует");
                }
                else
                {
                    uint id;
                    if (Users.Count == 0)
                    {
                        id = 1;
                    }
                    else
                    {
                        id = (uint)(Users.Count + 1);
                    }
                    var hashPassword = ControllerCryptograph.GenerateHash(password);
                    Users.Add(new User(id, new List<uint> { 0 }, login, hashPassword, DateTime.Now));
                    Console.WriteLine("Новый пользователь добавлен");
                    SaveAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }



        /// <summary>
        /// Привязка пользователя к группе по идентификатору группы
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idGroup"></param>
        public void AddInGroup(string login, uint idGroup)
        {

            var user = Users.SingleOrDefault(u => u.Login.Equals(login));

            if(idGroup > 0 && user != null)
            {
                user.IdGroup.Add(idGroup);
                Console.WriteLine("Пользователь добавлен в группу");
                SaveAsync();
            }
            else
            {
                Console.WriteLine("Пользователь не найден или вы присвоили недопустимый идентификатор группы");
            }
        }

        public void DeleteUser(string login)
        {
            var user = Users.SingleOrDefault(u => u.Login.Equals(login));

            if(user != null  && !login.Equals("root"))
            {
                Users.Remove(user);
                SaveAsync();
                Console.WriteLine("Пользователь удалён из системы");
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
        }

        /// <summary>
        /// Показывает всех пользователей, которые состоят в группе с указанным идентификатором
        /// </summary>
        /// <param name="idGroup"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsersOfGroup(uint idGroup)
        {
            return Users.Where(u => u.IdGroup.Contains(idGroup));
        }

        /// <summary>
        /// Получить данные пользователя.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        private async Task<List<User>> GetUsersDataAsync()
        {
            return await LoadAsync<List<User>>(USERS_FILE_NAME) ?? new List<User>();
        }

        /// <summary>
        /// Сохранить данные пользователя.
        /// </summary>
        public async void SaveAsync()
        {
            await SaveAsync(USERS_FILE_NAME, Users);
        }

    }
}
