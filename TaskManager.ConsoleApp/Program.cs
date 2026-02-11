using System.Text;
using KMA.TaskManager.Services;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var projectService = new ProjectService();
            var taskService = new TaskService();
            while (true)
            {
                ClearScreen();

                var projects = projectService.GetAllProjects();
                ShowProjects(projects);

                int selectedIndex = ReadProjectIndex(projects.Count);

                ClearScreen();

                var selectedProject = projects[selectedIndex - 1];
                Console.WriteLine($"Проєкт №{selectedIndex}");
                Console.WriteLine(selectedProject);

                Console.WriteLine();
                Console.WriteLine("Натисніть клавішу Enter, щоб повернутись до меню, або ввдеіть exit для виходу з програми.");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
            }
        }

        private static void ClearScreen()
        {
            Console.Write("\x1b[3J"); // ANSI-command to clear scrollback
            Console.Clear();
        }

        private static void ShowProjects(List<ProjectUIModel> projects)
        {
            for (int i = 0; i < projects.Capacity; i++)
            {
                Console.WriteLine($"{i + 1}. {projects[i].Name}");
            }
        }

        private static int ReadProjectIndex(int projectsCount)
        {
            while (true)
            {
                Console.Write("Введіть номер проєкту, який хочете побачити детальніше: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int index) &&
                    index > 0 &&
                    index <= projectsCount)
                {
                    return index;
                }

                Console.WriteLine("Некоректний номер. Спробуйте ще раз.");
            }
        }

    }
}
