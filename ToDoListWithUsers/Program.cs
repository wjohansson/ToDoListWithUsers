using System.Diagnostics;

namespace ToDoListWithUsers
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Kvar:
            // Tvåfaktorsautentisering

            // Kolla igenom alla funktioner så att kopplingen mellan allt inte förstörts
            // Kollat igenom fram till loggat in -> alla to do funktioner fungerar -> remove dir fungerar -> fortsätt kolla ifall
            //  allt efter users menu fungerar fortfarande, försök nå alla endpoints och hitta alla buggar
            
            UserManager userManager = new UserManager();

            MenuManager menuManager = new MenuManager();
            menuManager.LoginMenu();

        }
    }
}
