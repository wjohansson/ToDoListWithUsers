namespace ToDoListWithUsers
{
    public class CategoryManager : ToDoFileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }
        public override string DatabasePath { get; set; }
        public override string Username { get; set; }

        public CategoryManager(User user) : base(user)
        {
        }

        public override void Create(User user)
        {
            Username = user.Username;
            FileName = @"\ToDoListCategory.json";

            Path = Directory.GetParent(_currentDir).Parent.Parent.FullName + $@"\Database\{Username}" + FileName;

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)) || File.ReadAllText(Path) == "[]")
            {
                using (FileStream fs = File.Create(Path)) { }

                File.WriteAllText(Path, @"[{""Title"":""No category"",""Category"":null,""Id"":0,""Tasks"":null}]");
            }
        }

        public override void ClearLists(ToDoFileManager fileManager)
        {
            string[] fileType = fileManager.GetType().ToString().Split(".");

            if (TaskManager.AreYouSure($"Are you sure you want to delete ALL {fileType[1]} lists, this can not be undone? y/N: "))
            {
                fileManager.Lists.Clear();
                fileManager.Update();

                File.WriteAllText(Path, @"[{""Title"":""No category"",""Category"":null,""Id"":0,""Tasks"":null}]");
                Lists = Get();
            }
        }
    }
}

