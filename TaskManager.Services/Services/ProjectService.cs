using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IStorageContext _storageContext;
        private readonly IProjectMapper _projectMapper;

        // Впровадження залежності через конструктор(Constructor Injection)
        public ProjectService(IStorageContext storageContext, IProjectMapper projectMapper)
        {
            _storageContext = storageContext;
            _projectMapper = projectMapper;
        }

        public List<ProjectUIModel> GetAllProjects()
        {
            var projectDataModels = _storageContext.GetProjects();

            // Для кожного проєкту рахуємо статистику та перетворюємо в UI-модель
            return projectDataModels.Select(project =>
            {
                // Рахуємо загальну кількість завдань для цього проєкту
                var totalTasks = _storageContext.GetTasksCountByProjectId(project.Id);

                // Рахуємо кількість завершених завдань для розрахунку прогресу
                var completedTasks = _storageContext.GetTasksByProjectId(project.Id)
                    .Count(t => t.IsCompleted);

                // Використовуємо мапер для створення UI-моделі
                return _projectMapper.MapToUI(project, totalTasks, completedTasks);
            }).ToList();
        }

        public ProjectUIModel? GetProjectById(Guid id)
        {
            var projectData = _storageContext.GetProjectById(id);
            if (projectData == null) return null;

            var tasks = _storageContext.GetTasksByProjectId(id).ToList();
            int total = tasks.Count;
            int completed = tasks.Count(t => t.IsCompleted);

            return _projectMapper.MapToUI(projectData, total, completed);
        }
    }
}
