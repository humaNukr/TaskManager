using System.Text;
using KMA.TaskManager.Services;

namespace TaskManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            while (true)
            {
                //Console.Clear();
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }

                var projectService = new ProjectService();
                var projects = projectService.GetAllProjects();
                foreach (var project in projects)
                {
                    Console.WriteLine(project);
                }

            }
        }
    }
}
