namespace ToDoListWithUsers
{
    public class SubTask
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public SubTask()
        {

        }

        public SubTask(string subTaskTitle, string subTaskDescription)
        {
            Title = subTaskTitle;
            Description = subTaskDescription;
        }
    }
}

