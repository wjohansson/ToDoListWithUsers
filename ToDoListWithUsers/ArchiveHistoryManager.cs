namespace ToDoListWithUsers
{
    public class ArchiveHistoryManager : ToDoFileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }
        public override string DatabasePath { get; set; }
        public override string Username { get; set; }

        public ArchiveHistoryManager(User user) : base(user)
        {
        }

        public override void Create(User user)
        {
            FileName = @"\ToDoListArchiveHistory.json";

            base.Create(user);
        }
    }
}

