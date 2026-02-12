using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Services;
using KMA.TaskManager.UIModels;
using System.Text;

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
                PrintColored("=== МЕНЕДЖЕР ЗАВДАНЬ===", ConsoleColor.Cyan);

                var projects = projectService.GetAllProjects();
                ShowProjects(projects);

                int selectedIndex = ReadProjectIndex(projects.Count);

                ClearScreen();
                var selectedProject = projects[selectedIndex - 1];

                PrintColored($"Проєкт №{selectedIndex}", ConsoleColor.Yellow);
                Console.WriteLine(selectedProject);

                var tasks = taskService.GetTasksByProjectId(selectedProject.Id);

                if (tasks.Count > 0)
                {
                    PrintColored("\nСПИСОК ЗАВДАНЬ (коротко):", ConsoleColor.White);
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {tasks[i]}");
                    }


                    HandleTaskDetails(tasks, taskService);
                }
                else
                {
                    PrintColored("\nУ цього проєкту ще немає завдань.", ConsoleColor.DarkGray);
                }

                PrintColored("\nУведіть будь що, щоб повернутись до меню, або 'exit' для виходу.", ConsoleColor.Gray);
                string input = Console.ReadLine()?.ToLower();
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
            for (int i = 0; i < projects.Count; i++)
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

        private static void PrintColored(string text, ConsoleColor color, string prefix = "")
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{prefix}{text}");
            Console.ResetColor();
        }

        private static void HandleTaskDetails(List<TaskUIModel> tasks, TaskService service)
        {
            PrintColored("\nВведіть номер завдання для повного опису (або Enter):", ConsoleColor.DarkCyan);
            var input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index > 0 && index <= tasks.Count)
            {
                ClearScreen();
                PrintColored("=== ДЕТАЛЬНА ІНФОРМАЦІЯ ПРО ЗАВДАННЯ ===", ConsoleColor.Magenta);
                var fullTask = service.GetTaskById(tasks[index - 1].Id);
                if (fullTask != null) PrintTaskDetailed(fullTask);
            }
        }

        private static ConsoleColor GetPriorityColor(TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.Critical => ConsoleColor.Red,
                TaskPriority.High => ConsoleColor.DarkRed,
                TaskPriority.Medium => ConsoleColor.Yellow,
                TaskPriority.Low => ConsoleColor.Blue,
                _ => ConsoleColor.White
            };
        }

        private static void PrintTaskDetailed(TaskUIModel task)
        {
            Console.WriteLine();
            PrintColored("┌──────────────────────────────────────────────────────────┐", ConsoleColor.Magenta);

            PrintColored($"  ЗАВДАННЯ: {task.Name.ToUpper()}", ConsoleColor.White);

            Console.Write("  Пріоритет: ");
            PrintColored(task.Priority.ToString(), GetPriorityColor(task.Priority));

            var dateColor = task.IsOverdue ? ConsoleColor.Red : ConsoleColor.Cyan;
            string dateWarning = task.IsOverdue ? " [ПРОСТРОЧЕНО!]" : "";

            Console.Write("  Термін до: ");
            PrintColored($"{task.DueDate:dd.MM.yyyy HH:mm}{dateWarning}", dateColor);

            string statusText = task.IsCompleted ? "✅ Виконано" : (task.IsOverdue ? "❌ Прострочено" : "⏳ В процесі");
            var statusColor = task.IsCompleted
                ? ConsoleColor.Green
                : (task.IsOverdue ? ConsoleColor.Red : ConsoleColor.Yellow);

            Console.Write("  Статус:    ");
            PrintColored(statusText, statusColor);

            PrintColored("├──────────────────────────────────────────────────────────┤", ConsoleColor.Magenta);
            PrintColored("  ОПИС:", ConsoleColor.DarkGray);

            Console.WriteLine($"  {task.Description}");

            PrintColored("└──────────────────────────────────────────────────────────┘", ConsoleColor.Magenta);
        }
    }
}