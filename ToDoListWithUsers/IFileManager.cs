
using System.Diagnostics;

namespace ToDoListWithUsers
{
    public interface IFileManager
    {
        static void Create()
        {

        }
        static void Update()
        {

        }
        static List<TaskList> Get()
        {
            return new List<TaskList>();
        }
    }
}
