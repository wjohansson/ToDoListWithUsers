using System.Text.Json;

namespace ToDoListWithUsers
{
    public class ArchiveManager : ToDoFileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }
        public override string DatabasePath { get; set; }
        public override string Username { get; set; }

        public ArchiveManager(User user) : base(user)
        {
        }

        public override void Create(User user)
        {
            FileName = @"\ToDoListArchive.json";

            base.Create(user);
        }
    }
}

