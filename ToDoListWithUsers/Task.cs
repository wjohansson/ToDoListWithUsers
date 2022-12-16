using System.Collections.Generic;

namespace ToDoListWithUsers
{
    public class Task
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public string Priority { get; set; }
        public string DateCreated { get; init; }
        public List<SubTask> SubTasks { get; set; }

        public Task()
        {

        }
        public Task(string taskTitle, string taskDescription, string priority)
        {
            Title = taskTitle;
            Description = taskDescription;
            Priority = priority;
            Completed = false;
            DateCreated = DateTime.Now.ToString("G");
            SubTasks = new List<SubTask>();
        }

        public void CreateSubTask(ToDoFileManager fileManager)
        {
            List<SubTask> subTasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks;

            string title;
            string description;
            try
            {
                title = TaskManager.CreateVariable("Enter a new sub-task: ", true, false, false, subTasks, null);
                description = TaskManager.CreateVariable("Enter the sub-task description (optional): ", false, false, false, null, null);
            }
            catch (Exception)
            {
                return;
            }

            SubTask newSubTask = new(title, description);

            subTasks.Add(newSubTask);

            fileManager.Update();
        }

        public void DeleteSubTask(ToDoFileManager fileManager)
        {
            List<SubTask> subTasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks;

            string position;

            try
            {
                position = TaskManager.CreateVariable("Position of the sub-task you want to delete: ", true, true, false, null, subTasks);
            }
            catch (Exception)
            {
                return;
            }

            if (TaskManager.AreYouSure("Are you sure you want to delete this sub-task? y/N: "))
            {
                subTasks.RemoveAt(Convert.ToInt32(position) - 1);

                fileManager.Update();
            }
        }

        public void Edit(ToDoFileManager fileManager)
        {
            List<Task> tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks;
            Task currentTask = tasks[TaskManager.taskPosition - 1];

            string title;
            string description;
            string priority;
            try
            {
                Console.WriteLine($"Old title: {currentTask.Title}");
                title = TaskManager.CreateVariable("Enter the new title or leave empty to keep old title: ", false, false, false, tasks, null);

                Console.WriteLine();
                Console.WriteLine($"Old description: {currentTask.Description}");
                description = TaskManager.CreateVariable("Enter the new title or leave empty to keep old title: ", false, false, false, null, null);

                Console.WriteLine();
                Console.WriteLine($"Old priority: {currentTask.Priority}");
                priority = TaskManager.CreateVariable("Enter the new priority or leave empty to keep old priority: ", false, true, true, null, null);
            }
            catch (Exception)
            {
                return;
            }

            if (TaskManager.AreYouSure("Are you sure you want to edit this task? y/N: "))
            {
                if (title != "Null")
                {
                    currentTask.Title = title;
                }

                if (description != "Null")
                {
                    currentTask.Description = description;
                }

                if (priority != "Null")
                {
                    currentTask.Priority = priority;
                }

                fileManager.Update();
            }
        }

        public void ViewSubTasks(ToDoFileManager fileManager)
        {
            Task currentTask = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1];

            Console.Clear();

            Console.WriteLine("TASK MENU");
            Console.WriteLine();

            if (currentTask.Completed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine($"Title - {currentTask.Title} (Prio: {currentTask.Priority})");
            Console.WriteLine($"    Description - {currentTask.Description}");

            Console.WriteLine();

            foreach (SubTask subTask in currentTask.SubTasks)
            {
                Console.WriteLine($"        Sub-task Position #{currentTask.SubTasks.IndexOf(subTask) + 1}");
                Console.WriteLine($"            Title - {subTask.Title}");
                Console.WriteLine($"                Description - {subTask.Description}");

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void EditSubTask(ToDoFileManager fileManager)
        {
            List<SubTask> subTasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks;

            string position;
            string title;
            string description;
            SubTask currentSubTask;

            try
            {
                position = TaskManager.CreateVariable("Position of the sub-task you want to edit: ", true, true, false, null, subTasks);

                currentSubTask = subTasks[Convert.ToInt32(position) - 1];

                Console.WriteLine();
                Console.WriteLine($"Old title: {currentSubTask.Title}");
                title = TaskManager.CreateVariable("Enter the new title or leave empty to keep old title: ", false, false, false, subTasks, null);

                Console.WriteLine();
                Console.WriteLine($"Old description: {currentSubTask.Description}");
                description = TaskManager.CreateVariable("Enter the new description or leave empty to keep old description: ", false, false, false, null, null);


            }
            catch (Exception)
            {
                return;
            }

            if (TaskManager.AreYouSure("Are you sure you want to edit this sub-task? y/N: "))
            {
                if (title != "Null")
                {
                    currentSubTask.Title = title;
                }

                if (description != "Null")
                {
                    currentSubTask.Description = description;
                }

                fileManager.Update();
            }
        }

        public bool ExistsContent(ToDoFileManager fileManager)
        {
            if (fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks.Count == 0)
            {
                Console.WriteLine("No content. Returning");

                Thread.Sleep(2000);

                return false;
            }

            return true;
        }

        public bool ToggleArchive(ToDoFileManager fileManager)
        {
            bool areYouSure = TaskManager.AreYouSure("Are you sure you want to toggle archivation of this task? y/N: ");

            if (!areYouSure)
            {
                return false;
            }

            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];
            Task currentTask = currentList.Tasks[TaskManager.taskPosition - 1];

            var listExists = false;
            var taskExists = false;

            List<Task> currentArchiveTasks;
            int archiveListPosition = 0;

            ToDoFileManager oppositeManager = TaskManager.GetOppositeManager(fileManager);

            foreach (TaskList list in oppositeManager.Lists)
            {
                if (list.Title == currentList.Title)
                {
                    listExists = true;
                    archiveListPosition = oppositeManager.Lists.IndexOf(list);
                    break;
                }
            }

            var taskList = new TaskList();

            if (listExists)
            {
                currentArchiveTasks = oppositeManager.Lists[archiveListPosition].Tasks;

                foreach (Task task in currentArchiveTasks)
                {
                    if (task.Title == currentTask.Title)
                    {
                        taskExists = true;
                        break;
                    }
                }

                if (taskExists)
                {
                    Console.WriteLine("Task with this name already exists.");

                    if (TaskManager.AreYouSure("Do you want to delete this task? y/N: "))
                    {
                        taskList.DeleteTask(fileManager);
                        fileManager.Update();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                oppositeManager.Lists[archiveListPosition].Tasks.Add(currentTask);
            }
            else
            {
                TaskList newList = new(currentList.Title, currentList.Category, currentList.Id, new List<Task>());

                oppositeManager.Lists.Add(newList);

                int newListPosition = oppositeManager.Lists.IndexOf(newList);

                oppositeManager.Lists[newListPosition].Tasks.Add(currentTask);
            }

            oppositeManager.Update();

            taskList.DeleteTask(fileManager);
            fileManager.Update();

            return true;
        }
    }
}

