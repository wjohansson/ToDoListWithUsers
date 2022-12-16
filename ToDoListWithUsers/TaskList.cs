using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListWithUsers
{
    public class TaskList
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
        public List<Task> Tasks { get; set; }

        public TaskList()
        {

        }

        public TaskList(string listTitle, string listCaterogy, int listId, List<Task> tasks)
        {
            Title = listTitle;
            Category = listCaterogy;
            Id = listId;
            Tasks = tasks;
        }

        public void CreateTask(ToDoFileManager fileManager)
        {
            List<Task> tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks;

            string title;
            string description;
            string priority;

            try
            {
                title = TaskManager.CreateVariable("Enter a new task: ", true, false, false, tasks, null);

                description = TaskManager.CreateVariable("Enter the task description (optional): ", false, false, false, null, null);

                priority = TaskManager.CreateVariable("Enter priority of the task as a number between 1 and 5 (optional): ", false, true, true, null, null);
            }
            catch (Exception)
            {
                return;
            }

            Task newTask = new(title, description, priority);

            tasks.Add(newTask);

            fileManager.Update();
        }

        public void DeleteTask(ToDoFileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            List<Task> tasks = currentList.Tasks;

            bool areYouSure = true;

            if (TaskManager.taskPosition == 0)
            {
                try
                {
                    string position = TaskManager.CreateVariable("Enter the position of the task you want to delete: ", true, true, false, null, tasks);

                    TaskManager.taskPosition = Convert.ToInt32(position);
                }
                catch (Exception)
                {
                    return;
                }

                areYouSure = TaskManager.AreYouSure("Are you sure you want to delete this task? y/N: ");

            }

            if (areYouSure)
            {
                tasks.RemoveAt(TaskManager.taskPosition - 1);

                fileManager.Update();
            }
        }

        public void Edit(ToDoFileManager fileManager, bool mustChangeTitle)
        {
            ToDoFileManager oppositeManager = TaskManager.GetOppositeManager(fileManager);

            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            string title;
            string category;

            try
            {
                Console.WriteLine();
                Console.WriteLine($"Old title: {currentList.Title}");
                if (mustChangeTitle)
                {
                    title = TaskManager.CreateVariable("Enter the new title: ", true, false, false, oppositeManager.Lists, null);
                }
                else
                {
                    title = TaskManager.CreateVariable("Enter the new title or leave empty to keep the old title: ", false, false, false, fileManager.Lists, null);
                }

                Console.WriteLine($"Old category: {currentList.Category}");

                var categoryManager = new CategoryManager(MenuManager.CurrentUserLoggedIn);
                var taskLists = new TaskLists();
                taskLists.ViewCategories(categoryManager);
                var temp = taskLists.ChooseCategory(categoryManager);

                if (temp == "")
                {
                    throw new Exception();
                }

                category = temp;
            }
            catch (Exception)
            {
                return;
            }

            if (mustChangeTitle || TaskManager.AreYouSure("Are you sure you want to edit this list? y/N: "))
            {
                if (title != "Null")
                {
                    currentList.Title = title;
                }

                if (category != "Null")
                {
                    currentList.Category = category;
                }

                fileManager.Update();
            }
        }

        public void ToggleCompleteTask(ToDoFileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            List<Task> tasks = currentList.Tasks;

            string position;

            try
            {
                position = TaskManager.CreateVariable("Enter the position of the task you want to toggle: ", true, true, false, null, tasks);
            }
            catch (Exception)
            {
                return;
            }

            Task currentTask = tasks[Convert.ToInt32(position) - 1];

            currentTask.Completed = !currentTask.Completed;

            fileManager.Update();
        }

        public void ViewTasksCollapsed(ToDoFileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            Console.Clear();

            Console.WriteLine("LIST MENU");
            Console.WriteLine();

            Console.WriteLine($"List Title - {currentList.Title} (Category: {currentList.Category})");
            Console.WriteLine();

            List<Task> tasks = currentList.Tasks;

            if (currentList.Tasks.Count == 0)
            {
                Console.WriteLine("No existing tasks.");
                Console.WriteLine();

                return;
            }

            foreach (Task task in tasks)
            {
                if (task.Completed)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"    Task Position #{tasks.IndexOf(task) + 1}");
                Console.WriteLine($"        Title - {task.Title}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public void ViewTasksExpanded(ToDoFileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            Console.Clear();

            Console.WriteLine("LIST MENU");
            Console.WriteLine();

            Console.WriteLine($"List Title - {currentList.Title} (Category: {currentList.Category})");
            Console.WriteLine();


            if (currentList.Tasks.Count == 0)
            {
                Console.WriteLine("No existing tasks.");
                Console.WriteLine();

                return;
            }

            foreach (Task task in currentList.Tasks)
            {
                if (task.Completed)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"    Task Position #{currentList.Tasks.IndexOf(task) + 1}");
                Console.WriteLine($"        Title - {task.Title} (Prio: {task.Priority})");
                Console.WriteLine($"            ¤ {task.Description}");
                Console.WriteLine();

                foreach (SubTask subTask in task.SubTasks)
                {
                    Console.WriteLine($"            Sub-Task Position #{task.SubTasks.IndexOf(subTask) + 1}");
                    Console.WriteLine($"                Title - {subTask.Title}");
                    Console.WriteLine($"                    ¤ {subTask.Description}");
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public bool ViewTask(ToDoFileManager fileManager)
        {
            List<Task> tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks;

            try
            {
                string position = TaskManager.CreateVariable("Enter the position of the task you want to view: ", true, true, false, null, tasks);

                TaskManager.taskPosition = Convert.ToInt32(position);
            }
            catch (Exception)
            {
                return false;
            }

            var task = new Task();
            task.ViewSubTasks(fileManager);

            return true;
        }

        public bool ExistsContent(ToDoFileManager fileManager)
        {
            if (fileManager.Lists[TaskManager.listPosition - 1].Tasks.Count == 0)
            {
                Console.WriteLine("No content. Returning");

                Thread.Sleep(2000);

                return false;
            }

            return true;
        }

        public bool ToggleArchive(ToDoFileManager fileManager)
        {
            bool areYouSure = TaskManager.AreYouSure("Are you sure you want to toggle archivation of this list? y/N: ");

            if (!areYouSure)
            {
                return false;
            }

            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            var listExists = false;

            ToDoFileManager oppositeManager = TaskManager.GetOppositeManager(fileManager);

            foreach (TaskList list in oppositeManager.Lists)
            {
                if (list.Title == currentList.Title)
                {
                    listExists = true;
                    break;
                }
            }

            if (listExists)
            {
                Console.WriteLine("List with this name already exists.");

                Edit(fileManager, true);
            }

            oppositeManager.Lists.Add(currentList);
            oppositeManager.Update();

            var taskLists = new TaskLists();
            taskLists.DeleteList(fileManager);

            return true;
        }

        public void Sort(ToDoFileManager fileManager)
        {
            Console.WriteLine("[N] To sort by name.");
            Console.WriteLine("[D] To sort by date.");
            Console.WriteLine("[P] To sort by priority.");
            Console.WriteLine("[C] To sort by completed.");
            Console.WriteLine("[T] To sort by number of sub-tasks.");
            Console.WriteLine("[B] To go back.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadKey().Key.ToString().ToUpper())
            {
                case "N":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.Title).ToList();


                    break;
                case "D":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.DateCreated).ToList();


                    break;
                case "P":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.Priority).ToList();


                    break;
                case "C":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderByDescending(o => o.Completed).ToList();


                    break;
                case "T":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.SubTasks.Count()).ToList();


                    break;
                case "B":
                    ViewTasksCollapsed(fileManager);
                    var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
                    taskManager.ListOptions(fileManager);

                    break;
            }
        }
    }
}

