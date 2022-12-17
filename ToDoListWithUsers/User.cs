
using System.ComponentModel.DataAnnotations;

namespace ToDoListWithUsers
{
    public class User
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^\\da-zA-Z])(.{10,})$", ErrorMessage = "Password must be atleast 10 characters, one uppercase, one lowercase, one number, and one special")]
        public string Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Age must be 1 or more")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Adress is required")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Permission is requierd")]
        public string Permission { get; set; }


        public User()
        {
            Username = "TempUsername";
            Password = "TempPassword1!";
            FirstName = "TempFirst";
            LastName = "TempLast";
            Email = "temp@mail.com";
            Age = 1;
            Gender = "TempGender";
            Adress = "TempAdress";
            Permission = "TempPermission";
        }

        public void IncreasePermission()
        {
            string userPermission = MenuManager.CurrentUser.Permission;
            string loggedInPermission = MenuManager.CurrentUserLoggedIn.Permission;
            Console.WriteLine();

            if (!MenuManager.AreYouSure("Are you sure you want to increase this users permission? y/N: "))
            {
                return;
            }

            Console.WriteLine();

            if (userPermission == "Admin")
            {
                Console.WriteLine("Users permission is already admin, can not increase. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (userPermission == "Moderator" && loggedInPermission == "Moderator")
            {
                Console.WriteLine("Logged in user can not promote to admin. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (userPermission == "Moderator" && (loggedInPermission == "Admin" || loggedInPermission == "System"))
            {
                MenuManager.CurrentUser.Permission = "Admin";
            }
            else if (userPermission == "User")
            {
                MenuManager.CurrentUser.Permission = "Moderator";
            }

            MenuManager.UserManager.Update();
        }

        public void ReducePermission()
        {
            string userPermission = MenuManager.CurrentUser.Permission;
            string loggedInPermission = MenuManager.CurrentUserLoggedIn.Permission;

            Console.WriteLine();

            if (!MenuManager.AreYouSure("Are you sure you want to reduce this users permission? y/N: "))
            {
                return;
            }

            Console.WriteLine();

            if (userPermission == "User")
            {
                Console.WriteLine("Users permission is already user, can not reduce. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if ((userPermission == "Admin" || userPermission == "Moderator") && loggedInPermission == "Moderator")
            {
                Console.WriteLine("Logged in user can not demote this user. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (userPermission == "Admin" && loggedInPermission == "Admin")
            {
                Console.WriteLine("Logged in user can not demote this user. Returning");
                Thread.Sleep(2000);
                return;
            }
            else if (userPermission == "Admin" && loggedInPermission == "System")
            {
                MenuManager.CurrentUser.Permission = "Moderator";
            }
            else if (userPermission == "Moderator")
            {
                MenuManager.CurrentUser.Permission = "User";
            }

            MenuManager.UserManager.Update();
        }

        public bool DeleteUser()
        {
            Console.WriteLine();

            if (!MenuManager.AreYouSure("Are you sure you want to delete this user? y/N: "))
            {
                return false;
            }

            List<User> users = MenuManager.UserManager.Users;

            ActiveManager activeManager = new(MenuManager.CurrentUser);
            activeManager.DeleteUserFolder();

            users.RemoveAt(MenuManager.userPosition);

            MenuManager.UserManager.Update();

            return true;
        }
    }
}
