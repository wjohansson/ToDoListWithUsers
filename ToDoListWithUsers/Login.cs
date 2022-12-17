
using System.Security.Cryptography;
using System.Threading.Tasks.Sources;

namespace ToDoListWithUsers
{
    public class Login
    {
        public bool UserLogin()
        {
            List<User> users = MenuManager.UserManager.Users;

            Console.Clear();

            string username;
            string password;

            while (true)
            {
                try
                {
                    username = MenuManager.CreateVariable("Enter your username: ", false, false, false);

                    foreach (User user in users)
                    {
                        if (user.Username == username)
                        {
                            MenuManager.userPosition = users.IndexOf(user);
                            MenuManager.CurrentUserLoggedIn = user;
                            break;
                        }
                    }

                    Console.Clear();

                    password = MenuManager.CreateVariable("Enter your password: ", false, false, true);

                    if (MenuManager.userPosition == -1 || !VerifyLogin(password, MenuManager.CurrentUserLoggedIn.Password, MenuManager.CurrentUserLoggedIn.PasswordSalt))
                    {
                        MenuManager.userPosition = -1;
                        MenuManager.CurrentUserLoggedIn = null;
                        Console.WriteLine();
                        Console.WriteLine("Incorrect username or password. Try again.");
                        Console.SetCursorPosition(0, 0);

                        continue;
                    }

                    break;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            MenuManager.userLoggedInPosition = MenuManager.userPosition;
            MenuManager.CurrentUserLoggedIn = MenuManager.UserManager.Users[MenuManager.userLoggedInPosition];

            return true;
        }

        public bool VerifyLogin(string password, string hash, byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }
    }
}
