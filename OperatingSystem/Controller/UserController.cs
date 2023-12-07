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

            CurrentUser = Users.SingleOrDefault(u => u.Login == login && ControllerCryptograph.ValidatePassword(password, u.HashPassword));

            if (CurrentUser == null)
            {
                var user = Users.OrderByDescending(x => x.Id).FirstOrDefault();
                uint id;
                if(user == null)
                {
                    id = 1;
                }
                else
                {
                    id = user.Id + 1;
                }
                var hashPassword = ControllerCryptograph.GenerateHash(password);
                CurrentUser = new User(id,new List<uint>() { 0 },login, hashPassword, DateTime.Now);
                Users.Add(CurrentUser);
                IsNewUser = true;
                SaveAsync();
            }

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
