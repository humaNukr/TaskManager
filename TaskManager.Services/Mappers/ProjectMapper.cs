using System;
using System.Collections.Generic;
using System.Linq;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        private double CalculateProgressFraction(int total, int completed)
        {
            return total == 0 ? 0 : (double)completed / total;
        }

        public ProjectListDTO MapToListDTO(ProjectDataModel project, int totalTasks, int completedTasks)
        {
            double fraction = CalculateProgressFraction(totalTasks, completedTasks);
            double progressPercentage = fraction * 100;

            return new ProjectListDTO(
                project.Id,
                project.Name,
                totalTasks,
                completedTasks,
                progressPercentage
            );
        }

        public ProjectDetailsDTO MapToDetailsDTO(ProjectDataModel project, IEnumerable<TaskListDTO> tasks)
        {
            int totalTasks = tasks?.Count() ?? 0;
            int completedTasks = tasks?.Count(t => t.IsCompleted) ?? 0;
            double fraction = CalculateProgressFraction(totalTasks, completedTasks);
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

        public ProjectDataModel MapToData(ProjectCreateModel model)
        {
            return new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ProjectType = model.ProjectType
            };
        }
    }
}