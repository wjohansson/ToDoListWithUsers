using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListWithUsers
{
    public class TaskManager
    {
        public static int listPosition = 0;
        public static int taskPosition = 0;

        private readonly Task _task;
        private readonly TaskList _taskList;
        private readonly TaskLists _taskLists;

        public TaskManager(Task task, TaskList taskList, TaskLists taskLists)
        {
            _task = task;
            _taskList = taskList;
            _taskLists = taskLists;
        }

        public void OverviewOptions(ToDoFileManager fileManager)
        {
            Console.WriteLine("[E] To expand all lists.");
            Console.WriteLine("[C] To collapse all lists.");
            Console.WriteLine("[V] To view a list and its tasks.");
            Console.WriteLine("[L] To view recently visited list.");
            Console.WriteLine("[D] To delete a list.");
            Console.WriteLine("[N] To create a new list.");
            Console.WriteLine("[X] To go to category menu.");
            Console.WriteLine("[S] To sort all lists.");
            Console.WriteLine("[A] To toggle between archive and overview menu.");
            Console.WriteLine("[DEL] To delete all lists and tasks.");
            Console.WriteLine("[B] To go back to User page.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadKey().Key.ToString().ToUpper())
            {
                case "E":
                    _taskLists.ViewListsExpanded(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "C":
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "V":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        if (_taskLists.ViewList(fileManager))
                        {
                            ListOptions(fileManager);
                            break;
                        }
                    }

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "L":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        int historyListId;

                        try
                        {
                            ToDoFileManager historyManager = GetHistoryManager(fileManager);

                            historyListId = historyManager.Lists[0].Id;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            _taskLists.ViewListsCollapsed(fileManager);
                            Console.WriteLine("Nothing to view. Returning");

                            Thread.Sleep(2000);

                            _taskLists.ViewListsCollapsed(fileManager);
                            OverviewOptions(fileManager);
                            break;
                        }

                        foreach (TaskList list in fileManager.Lists)
                        {
                            if (historyListId == list.Id)
                            {
                                listPosition = fileManager.Lists.IndexOf(list) + 1;
                                break;
                            }
                        }
                        _taskLists.ViewList(fileManager);
                        ListOptions(fileManager);
                    }
                    else
                    {
                        _taskLists.ViewListsCollapsed(fileManager);
                        OverviewOptions(fileManager);
                    }


                    break;
                case "D":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        _taskLists.DeleteList(fileManager);
                    }

                    listPosition = 0;

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "N":
                    _taskLists.ViewListsCollapsed(fileManager);
                    _taskLists.CreateList(fileManager);

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "X":
                    var categoryManager = new CategoryManager(MenuManager.CurrentUserLoggedIn);
                    _taskLists.ViewCategories(categoryManager);
                    CategoryOptions(categoryManager);

                    break;
                case "S":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        _taskLists.Sort(fileManager);
                    }

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "A":
                    var oppositeManager = GetOppositeManager(fileManager);
                    _taskLists.ViewListsCollapsed(oppositeManager);
                    OverviewOptions(oppositeManager);

                    break;
                case "DELETE":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        fileManager.ClearLists(fileManager);
                    }

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "B":
                    var menuManager = new MenuManager();
                    menuManager.UsersMenu();

                    break;
                case "Q":
                    _taskLists.ViewListsCollapsed(fileManager);
                    QuitProgram();

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                default:
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
            }
        }

        private void CategoryOptions(ToDoFileManager fileManager)
        {
            Console.WriteLine("[N] To create a new category.");
            Console.WriteLine("[D] To delete a category.");
            Console.WriteLine("[DEL] To delete all categories.");
            Console.WriteLine("[B] To go back to all lists.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadKey().Key.ToString().ToUpper())
            {
                case "N":
                    _taskLists.ViewCategories(fileManager);
                    _taskLists.CreateCategory(fileManager);

                    _taskLists.ViewCategories(fileManager);
                    CategoryOptions(fileManager);

                    break;
                case "D":
                    _taskLists.ViewCategories(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        _taskLists.DeleteCategory(fileManager);
                    }

                    _taskLists.ViewCategories(fileManager);
                    CategoryOptions(fileManager);

                    break;
                case "DELETE":
                    _taskLists.ViewCategories(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        fileManager.ClearLists(fileManager);
                    }

                    _taskLists.ViewCategories(fileManager);
                    CategoryOptions(fileManager);

                    break;
                case "B":
                    var activeManager = new ActiveManager(MenuManager.CurrentUserLoggedIn);
                    _taskLists.ViewListsCollapsed(activeManager);
                    OverviewOptions(activeManager);

                    break;
                case "Q":
                    _taskLists.ViewCategories(fileManager);
                    QuitProgram();

                    _taskLists.ViewCategories(fileManager);
                    CategoryOptions(fileManager);

                    break;
                default:
                    _taskLists.ViewCategories(fileManager);
                    CategoryOptions(fileManager);

                    break;
            }
        }

        public void ListOptions(ToDoFileManager fileManager)
        {
            Console.WriteLine("[E] To expand all tasks.");
            Console.WriteLine("[C] To collapse all tasks.");
            Console.WriteLine("[M] To modify this list.");
            Console.WriteLine("[A] To toggle archivation of this list.");
            Console.WriteLine("[V] To view a task and its sub-tasks.");
            Console.WriteLine("[D] To delete a task.");
            Console.WriteLine("[N] To create a new task.");
            Console.WriteLine("[T] To toggle completion of a task.");
            Console.WriteLine("[S] To sort tasks.");
            Console.WriteLine("[B] To go back to all lists.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadKey().Key.ToString().ToUpper())
            {
                case "E":
                    _taskList.ViewTasksExpanded(fileManager);
                    ListOptions(fileManager);

                    break;
                case "C":
                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "M":
                    _taskList.ViewTasksCollapsed(fileManager);
                    _taskList.Edit(fileManager, false);

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "A":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ToggleArchive(fileManager))
                    {
                        listPosition = 0;

                        _taskLists.ViewListsCollapsed(fileManager);
                        OverviewOptions(fileManager);
                        break;
                    }

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "V":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        if (_taskList.ViewTask(fileManager))
                        {
                            TaskOptions(fileManager);
                            break;
                        }
                    }

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "D":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.DeleteTask(fileManager);
                    }

                    taskPosition = 0;

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "N":
                    _taskList.ViewTasksCollapsed(fileManager);
                    _taskList.CreateTask(fileManager);

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "T":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.ToggleCompleteTask(fileManager);

                    }

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "S":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.Sort(fileManager);
                    }

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "B":
                    listPosition = 0;
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "Q":
                    _taskList.ViewTasksCollapsed(fileManager);
                    QuitProgram();

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                default:
                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
            }
        }

        public void TaskOptions(ToDoFileManager fileManager)
        {
            Console.WriteLine("[M] To modify this task.");
            Console.WriteLine("[A] To archive this task.");
            Console.WriteLine("[D] To delete a sub-task.");
            Console.WriteLine("[S] To edit a sub-task.");
            Console.WriteLine("[N] To create a new sub-task.");
            Console.WriteLine("[B] To go back to list overview.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");

            switch (Console.ReadKey().Key.ToString().ToUpper())
            {
                case "M":
                    _task.ViewSubTasks(fileManager);
                    _task.Edit(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "A":
                    _task.ViewSubTasks(fileManager);

                    if (_task.ToggleArchive(fileManager))
                    {
                        taskPosition = 0;

                        _taskList.ViewTasksCollapsed(fileManager);
                        ListOptions(fileManager);
                        break;
                    }

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "D":
                    _task.ViewSubTasks(fileManager);

                    if (_task.ExistsContent(fileManager))
                    {
                        _task.DeleteSubTask(fileManager);
                    }

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "S":
                    _task.ViewSubTasks(fileManager);
                    _task.EditSubTask(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;

                case "N":
                    _task.ViewSubTasks(fileManager);
                    _task.CreateSubTask(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "B":
                    taskPosition = 0;
                    _taskLists.ViewList(fileManager);
                    ListOptions(fileManager);

                    break;
                case "Q":
                    _task.ViewSubTasks(fileManager);
                    QuitProgram();

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                default:
                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
            }
        }

        public static ToDoFileManager GetHistoryManager(ToDoFileManager fileManager)
        {
            ToDoFileManager historyManager = null;

            if (fileManager.GetType() == new ActiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                historyManager = new HistoryManager(MenuManager.CurrentUserLoggedIn);
            }
            else if (fileManager.GetType() == new ArchiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                historyManager = new ArchiveHistoryManager(MenuManager.CurrentUserLoggedIn);
            }

            return historyManager;
        }

        public static ToDoFileManager GetOppositeManager(ToDoFileManager fileManager)
        {
            ToDoFileManager oppositeManager = null;

            if (fileManager.GetType() == new ActiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                oppositeManager = new ArchiveManager(MenuManager.CurrentUserLoggedIn);
            }
            else if (fileManager.GetType() == new ArchiveManager(MenuManager.CurrentUserLoggedIn).GetType())
            {
                oppositeManager = new ActiveManager(MenuManager.CurrentUserLoggedIn);
            }

            return oppositeManager;
        }

        public static void RemoveListFromHistory(ToDoFileManager fileManager)
        {
            int listId = fileManager.Lists[listPosition - 1].Id;

            ToDoFileManager historyManager = GetHistoryManager(fileManager);

            historyManager.Lists.RemoveAll(TaskList => TaskList.Id == listId);

            historyManager.Update();
        }

        public static string CreateVariable(string message, bool isMandatory, bool isInt, bool isPrio, dynamic? duplicateCheck, dynamic? positionCheck)
        {
            Console.WriteLine();
            Console.WriteLine("[0] To go back");
            Console.WriteLine();

            while (true)
            {
                Console.Write(message);
                string newVariable = Console.ReadLine();

                if (newVariable == "0")
                {
                    throw new Exception();
                }

                if (isMandatory && String.IsNullOrWhiteSpace(newVariable))
                {
                    Console.WriteLine("Can not be empty. Try again");

                    continue;
                }
                else if (!isMandatory && String.IsNullOrWhiteSpace(newVariable))
                {
                    newVariable = "Null";
                }

                if (duplicateCheck != null)
                {
                    var exists = false;

                    foreach (var obj in duplicateCheck)
                    {
                        if (obj.Title == newVariable)
                        {
                            Console.WriteLine("Already exists. Try again");
                            exists = true;
                            break;
                        }
                    }

                    if (exists)
                    {
                        continue;
                    }

                    return newVariable;
                }

                if (isPrio)
                {
                    try
                    {
                        if (newVariable != "Null")
                        {
                            int temp = Convert.ToInt32(newVariable);

                            if (temp > 5 || temp < 1)
                            {
                                throw new FormatException();
                            }
                        }
                        return newVariable;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Priority must be a number between 1 and 5. Try again");
                        continue;
                    }

                }

                if (isInt && isMandatory)
                {
                    try
                    {
                        int temp = Convert.ToInt32(newVariable);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Position must be a number. Try again");
                        continue;
                    }
                }

                if (positionCheck != null)
                {
                    try
                    {
                        var temp = positionCheck[Convert.ToInt32(newVariable) - 1];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Does not exist. Try again");
                        continue;
                    }
                }

                return newVariable;
            }

        }

        public static bool AreYouSure(string message)
        {
            Console.Write(message);

            switch (Console.ReadLine().ToUpper())
            {
                case "Y":
                    return true;
                default:
                    return false;
            }
        }

        public static void QuitProgram()
        {
            Console.Write("Are you sure you want to quit the program? Y/n: ");

            switch (Console.ReadLine().ToUpper())
            {
                case "N":
                    break;
                default:
                    Environment.Exit(-1);

                    break;
            }
        }
    }
}

