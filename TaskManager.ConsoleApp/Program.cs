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
                PrintColored("===МЕНЕДЖЕР ЗАВДАНЬ===", ConsoleColor.Cyan);

                var projects = projectService.GetAllProjects();
                ShowProjects(projects);

                PrintColored("\nВведіть номер проєкту або 'exit' для виходу:", ConsoleColor.Gray);
                string input = Console.ReadLine()?.ToLower();

                if (input == "exit") break;

                if (int.TryParse(input, out int index) && index > 0 && index <= projects.Count)
                {
                    var selectedProject = projects[index - 1];
                    var tasks = taskService.GetTasksByProjectId(selectedProject.Id);

                    HandleTaskDetails(selectedProject, tasks, taskService);
                }
                else
                {
                    PrintColored("Некоректний номер. Спробуйте ще раз.", ConsoleColor.Red, "Помилка: ");
                    Thread.Sleep(1200);
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
                Console.WriteLine($"{i + 1}. {projects[i]}");
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

        private static void HandleTaskDetails(ProjectUIModel project, List<TaskUIModel> tasks, TaskService service)
        {
            while (true)
            {
                ClearScreen();
                PrintProjectDetailed(project, tasks);

                PrintColored("\nВведіть номер завдання для повного опису або Enter, щоб змінити проєкт:", ConsoleColor.DarkCyan);
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                if (int.TryParse(input, out int index) && index > 0 && index <= tasks.Count)
                {
                    ClearScreen();
                    PrintColored("=== ПОВНА ІНФОРМАЦІЯ ПРО ЗАВДАННЯ ===", ConsoleColor.Magenta);

                    var fullTask = service.GetTaskById(tasks[index - 1].Id);
                    if (fullTask != null)
                    {
                        PrintTaskDetailed(fullTask);

                        PrintColored("\nНатисніть будь-яку клавішу, щоб повернутись до завдань проєкту...", ConsoleColor.DarkGray);
                        Console.ReadKey();
                    }
                }
                else
                {
                    PrintColored("Некоректний номер. Спробуйте ще раз.", ConsoleColor.Red, "Помилка: ");
                    Thread.Sleep(1200);
                }
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
            var color = ConsoleColor.Magenta;

            PrintBorderLine('╔', '═', '╗', color);
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

            PrintBorderLine('╠', '═', '╣', color);
            PrintColored("  ОПИС:", ConsoleColor.DarkGray);

            Console.WriteLine($"  {task.Description}");
            PrintBorderLine('╚', '═', '╝', color);
        }

        private static void PrintProjectDetailed(ProjectUIModel project, List<TaskUIModel> tasks)
        {
            Console.WriteLine();
            var color = ConsoleColor.DarkYellow;

            PrintBorderLine('╔', '═', '╗', color);
            PrintColored($"  ПРОЄКТ: {project.Name.ToUpper()}", ConsoleColor.White);
            PrintColored($"  Тип:    {project.ProjectType}", ConsoleColor.DarkGray);

            int barLength = 20;
            int filledParts = (int)(project.Progress / (100.0 / barLength));
            string bar = new string('█', filledParts) + new string('░', barLength - filledParts);
            Console.Write("  Прогрес: ");
            PrintColored($"[{bar}] {project.Progress:F1}%", ConsoleColor.DarkCyan);
            PrintColored($"           ({project.CompletedTasksCount} з {project.TotalTasksCount} завершено)", ConsoleColor.Gray);

            PrintBorderLine('╠', '═', '╣', color);
            PrintColored("  ОПИС:", ConsoleColor.DarkGray);
            Console.WriteLine($"  {project.Description}");

            PrintBorderLine('╠', '═', '╣', color);

            if (tasks.Count > 0)
            {
                PrintColored("  СПИСОК ЗАВДАНЬ:", ConsoleColor.White);
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {tasks[i]}");
                }
            }
            else
            {
                PrintColored("  Завдань у цьому проєкті ще немає.", ConsoleColor.DarkGray);
            }

            PrintBorderLine('╚', '═', '╝', color);
        }

        private static void PrintBorderLine(char left, char horizontal, char right, ConsoleColor color)
        {
            int width = Console.WindowWidth - 2;

            if (width < 2) width = 2;

            string line = left + new string(horizontal, width - 2) + right;
            PrintColored(line, color);
        }
    }
}