using System.Text.Json;

namespace ToDoListWithUsers
{
    public abstract class ToDoFileManager : IStateManager
    {
        public readonly string _currentDir = Environment.CurrentDirectory;
        public abstract string Path { get; set; }
        public abstract string DatabasePath { get; set; }
        public abstract string Username { get; set; }
        public abstract string FileName { get; set; }
        public abstract List<TaskList> Lists { get; set; }

        public ToDoFileManager(User user)
        {
            Create(user);
            Lists = Get();
        }

        public virtual void Create(User user)
        {
            Username = user.Username;

            DatabasePath = Directory.GetParent(_currentDir).Parent.Parent.FullName + @$"\Database\{Username}";
            Path = DatabasePath + FileName;

            if (!Directory.Exists(DatabasePath))
            {
                Directory.CreateDirectory(DatabasePath);
            }

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)))
            {
                using (FileStream fs = File.Create(Path)) { }

                File.WriteAllText(Path, "[]");
            }
        }

        public List<TaskList> Get()
        {
            string jsonData = File.ReadAllText(Path);

            List<TaskList> lists = JsonSerializer.Deserialize<List<TaskList>>(jsonData);

            return lists;
        }

        public void Update()
        {
            string jsonData = JsonSerializer.Serialize(Lists);

            File.WriteAllText(Path, jsonData);
        }

        public virtual void ClearLists(ToDoFileManager fileManager)
        {
            string[] fileType = fileManager.GetType().ToString().Split(".");

            if (TaskManager.AreYouSure($"Are you sure you want to delete ALL {fileType[1]} lists, this can not be undone? y/N: "))
            {
                fileManager.Lists.Clear();
                fileManager.Update();

                ToDoFileManager historyManager = TaskManager.GetHistoryManager(fileManager);
                historyManager.Lists.Clear();
                historyManager.Update();
            }
        }

        public virtual void DeleteUserFolder()
        {
            if (Directory.Exists(DatabasePath))
            {
                Directory.Delete(DatabasePath, true);
            }
        }
    }
}

