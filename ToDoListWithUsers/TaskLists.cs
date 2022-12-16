using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace ToDoListWithUsers
{
    public class TaskLists
    {
        public void CreateList(ToDoFileManager fileManager)
        {
            string title;
            string category;

            try
            {
                title = TaskManager.CreateVariable("Enter new list title: ", true, false, false, fileManager.Lists, null);

                var categoryManager = new CategoryManager(MenuManager.CurrentUserLoggedIn);

                ViewCategories(categoryManager);
                var temp = ChooseCategory(categoryManager);

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

            ToDoFileManager oppositeManager = TaskManager.GetOppositeManager(fileManager);

            var id = 1;

            foreach (TaskList list in fileManager.Lists)
            {
                if (list.Id >= id)
                {
                    id = list.Id + 1;
                }
            }

            foreach (TaskList list in oppositeManager.Lists)
            {
                if (list.Id >= id)
                {
                    id = list.Id + 1;
                }
            }

            TaskList newList = new(title, category, id, new List<Task>());

            fileManager.Lists.Add(newList);

            fileManager.Update();

        }

        public void ViewListsCollapsed(ToDoFileManager fileManager)
        {
            Console.Clear();

            if (fileManager.GetType() == new ActiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                Console.WriteLine("OVERVIEW MENU");
            }
            else if (fileManager.GetType() == new ArchiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                Console.WriteLine("ARCHIVE MENU");
            }

            Console.WriteLine();

            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No existing lists");
                Console.WriteLine();

                return;
            }

            Console.WriteLine("Current existing lists:");
            Console.WriteLine();

            foreach (TaskList list in fileManager.Lists)
            {
                var allTasksCompleted = true;

                if (list.Tasks.Count == 0)
                {
                    allTasksCompleted = false;
                }

                foreach (Task task in list.Tasks)
                {
                    if (!task.Completed)
                    {
                        allTasksCompleted = false;
                    }
                }

                if (allTasksCompleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }


                Console.WriteLine($"List Position #{fileManager.Lists.IndexOf(list) + 1}");
                Console.WriteLine($"    Title - {list.Title}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void ViewListsExpanded(ToDoFileManager fileManager)
        {
            Console.Clear();

            if (fileManager.GetType() == new ActiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                Console.WriteLine("OVERVIEW MENU");
            }
            else if (fileManager.GetType() == new ArchiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                Console.WriteLine("ARCHIVE MENU");
            }

            Console.WriteLine();

            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No existing lists.");
                Console.WriteLine();

                return;
            }
            Console.WriteLine("Current existing lists expanded:");
            Console.WriteLine();

            foreach (TaskList list in fileManager.Lists)
            {
                var allTasksCompleted = true;

                if (list.Tasks.Count == 0)
                {
                    allTasksCompleted = false;
                }

                foreach (Task task in list.Tasks)
                {
                    if (!task.Completed)
                    {
                        allTasksCompleted = false;
                    }
                }

                if (allTasksCompleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"List Position #{fileManager.Lists.IndexOf(list) + 1}");
                Console.WriteLine($"    Title - {list.Title} (Category: {list.Category})");

                Console.ForegroundColor = ConsoleColor.White;

                foreach (Task task in list.Tasks)
                {
                    if (task.Completed)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.WriteLine($"        Task Position #{list.Tasks.IndexOf(task) + 1}");
                    Console.WriteLine($"            Title - {task.Title} (Prio: {task.Priority})");
                    Console.WriteLine($"                ¤ {task.Description}");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        public bool ViewList(ToDoFileManager fileManager)
        {
            if (TaskManager.listPosition == 0)
            {
                try
                {
                    string position = TaskManager.CreateVariable("Enter the position of the list you want to view: ", true, true, false, null, fileManager.Lists);

                    TaskManager.listPosition = Convert.ToInt32(position);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            var historyManager = TaskManager.GetHistoryManager(fileManager);
            historyManager.Lists.Insert(0, fileManager.Lists[TaskManager.listPosition - 1]);
            historyManager.Update();

            var taskList = new TaskList();
            taskList.ViewTasksCollapsed(fileManager);

            return true;
        }

        public void DeleteList(ToDoFileManager fileManager)
        {
            bool areYouSure = true;

            if (TaskManager.listPosition == 0)
            {
                try
                {
                    string position = TaskManager.CreateVariable("Enter the position of the list you want to delete: ", true, true, false, null, fileManager.Lists);

                    TaskManager.listPosition = Convert.ToInt32(position);
                }
                catch (Exception)
                {
                    return;
                }

                areYouSure = TaskManager.AreYouSure("Are you sure you want to delete this list? y/N: ");


            }

            if (areYouSure)
            {
                TaskManager.RemoveListFromHistory(fileManager);

                fileManager.Lists.RemoveAt(TaskManager.listPosition - 1);

                fileManager.Update();
            }
        }

        public bool ExistsContent(ToDoFileManager fileManager)
        {
            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No content. Returning");

                Thread.Sleep(2000);

                return false;
            }

            return true;
        }

        public void ViewCategories(ToDoFileManager fileManager)
        {
            Console.Clear();

            Console.WriteLine("CATEGORY MENU");
            Console.WriteLine();

            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No existing categories");
                Console.WriteLine();

                return;
            }

            Console.WriteLine("Current existing caterogies:");
            Console.WriteLine();

            foreach (TaskList list in fileManager.Lists)
            {
                Console.WriteLine($"Category position #{fileManager.Lists.IndexOf(list) + 1}");
                Console.WriteLine($"    Title - {list.Title}");
                Console.WriteLine();
            }
        }

        public string ChooseCategory(ToDoFileManager fileManager)
        {
            string position;
            try
            {
                position = TaskManager.CreateVariable("Enter the position of the category you want: ", true, true, false, null, fileManager.Lists);
            }
            catch (Exception)
            {
                return "";
            }

            string category = fileManager.Lists[Convert.ToInt32(position) - 1].Title;

            return category;
        }

        public void CreateCategory(ToDoFileManager fileManager)
        {
            string title;
            try
            {
                title = TaskManager.CreateVariable("Enter new list category: ", true, false, false, fileManager.Lists, null);
            }
            catch (Exception)
            {
                return;
            }

            TaskList newList = new()
            {
                Title = title
            };

            fileManager.Lists.Add(newList);

            fileManager.Update();
        }

        public void DeleteCategory(ToDoFileManager fileManager)
        {
            string position;

            try
            {
                position = TaskManager.CreateVariable("Enter position of the category you want to delete: ", true, true, false, null, fileManager.Lists);

                if (position == "1")
                {
                    Console.WriteLine("Can not delete the first category. ");

                    DeleteCategory(fileManager);
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            if (TaskManager.AreYouSure("Are you sure you want to delete this category? y/N: "))
            {
                fileManager.Lists.RemoveAt(Convert.ToInt32(position) - 1);

                fileManager.Update();
            }
        }

        public void Sort(ToDoFileManager fileManager)
        {
            Console.WriteLine("[N] To sort by name.");
            Console.WriteLine("[C] To sort by category.");
            Console.WriteLine("[T] To sort by number of tasks.");
            Console.WriteLine("[B] To go back");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadKey().Key.ToString().ToUpper())
            {
                case "N":
                    fileManager.Lists = fileManager.Lists.OrderBy(o => o.Title).ToList();

                    break;
                case "C":
                    fileManager.Lists = fileManager.Lists.OrderBy(o => o.Category).ToList();

                    break;
                case "T":
                    fileManager.Lists = fileManager.Lists.OrderByDescending(o => o.Tasks.Count()).ToList();

                    break;
                case "B":
                    ViewListsCollapsed(fileManager);
                    var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
                    taskManager.OverviewOptions(fileManager);

                    break;
            }

            fileManager.Update();
        }
    }
}

