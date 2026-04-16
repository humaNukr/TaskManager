using System.Collections.Generic;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services.Mappers
{
    public static class ProjectMapper
    {
        // Допоміжний приватний метод, щоб формула існувала лише в одному місці
        private static double CalculateProgressFraction(int total, int completed)
        {
            return total == 0 ? 0 : (double)completed / total;
        }

        public static ProjectListDTO ToListDTO(ProjectDataModel project, int totalTasks, int completedTasks)
        {
            double fraction = CalculateProgressFraction(totalTasks, completedTasks);
            double progressPercentage = fraction * 100; // Робимо відсотки для списку

            return new ProjectListDTO(
                project.Id,
                project.Name,
                totalTasks,
                completedTasks,
                progressPercentage
            );
        }

        public static ProjectDetailsDTO ToDetailsDTO(ProjectDataModel project, IEnumerable<TaskListDTO> tasks)
        {
            int totalTasks = tasks?.Count() ?? 0;
            int completedTasks = tasks?.Count(t => t.IsCompleted) ?? 0;
            double fraction = CalculateProgressFraction(totalTasks, completedTasks); // Використовуємо фракцію для деталей
            string stats = $"{completedTasks} з {totalTasks} завдань завершено";

            return new ProjectDetailsDTO(
                project.Id,
                project.Name,
                project.Description,
                project.ProjectType,
                tasks,
                totalTasks,
                completedTasks,
                fraction,
                stats
            );
        }
    }
}