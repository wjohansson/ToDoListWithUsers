
using System.Dynamic;
using System.Reflection;
using System.Security;
using System.Text.Json;

namespace ToDoListWithUsers
{
    public class UserManager : IStateManager
    {
        public readonly string _currentDir = Environment.CurrentDirectory;
        public string Path { get; set; }
        public string FileName { get; set; }
        public List<User> Users { get; set; }

        public UserManager()
        {
            Create();
            Users = Get();
        }

        public void Create()
        {
            FileName = @"\Users.json";

            Path = Directory.GetParent(_currentDir).Parent.Parent.FullName + FileName;

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)) || File.ReadAllText(Path).Trim() == "[]")
            {
                File.WriteAllText(Path, "[]");

                var createUser = new CreateUser();
                string password = createUser.HashAndSaltPassword("S", out var salt);

                User newUser = new()
                {
                    Username = "S",
                    Password = password,
                    PasswordSalt = salt,
                    FirstName = "System",
                    LastName = "Manager",
                    Email = "system@mail.com",
                    Age = 9000,
                    Gender = "Robot",
                    Adress = "113.56.121.167",
                    Permission = "System"
                };

                Users = Get();

                Users.Add(newUser);
                Update();
            }
        }

        public List<User> Get()
        {
            string jsonData = File.ReadAllText(Path);
            List<User> lists = JsonSerializer.Deserialize<List<User>>(jsonData);

            return lists;
        }

        public void Update()
        {
            string jsonData = JsonSerializer.Serialize(Users, new JsonSerializerOptions() { WriteIndented = true });

            File.WriteAllText(Path, jsonData);
        }
    }
}
