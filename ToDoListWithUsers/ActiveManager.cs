using System.Text.Json;

namespace ToDoListWithUsers
{
    public class ActiveManager : ToDoFileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }
        public override string DatabasePath { get; set; }
        public override string Username { get; set; }

        public ActiveManager(User user) : base(user)
        {
        }

        public override void Create(User user)
        {
            FileName = @"\ToDoList.json";

            base.Create(user);
        }
    }
}

