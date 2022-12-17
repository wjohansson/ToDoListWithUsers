using System.Diagnostics;

namespace ToDoListWithUsers
{
    public class Program
    {
        static void Main(string[] args)
        {
            UserManager userManager = new UserManager();

            MenuManager menuManager = new MenuManager();
            menuManager.LoginMenu();

        }
    }
}
