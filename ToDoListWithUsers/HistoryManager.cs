using System.IO.Enumeration;
using System.Text.Json;

namespace ToDoListWithUsers
{
    public class HistoryManager : ToDoFileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }
        public override string DatabasePath { get; set; }
        public override string Username { get; set; }

        public HistoryManager(User user) : base(user)
        {
        }

        public override void Create(User user)
        {
            FileName = @"\ToDoListHistory.json";

            base.Create(user);
        }

    }
}

