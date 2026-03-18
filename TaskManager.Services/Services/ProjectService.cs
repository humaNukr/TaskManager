using System;
using System.Collections.Generic;
using System.Linq;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectMapper _projectMapper;
        private readonly ITaskMapper _taskMapper;

        // Впровадження залежності через конструктор(Constructor Injection)
        public ProjectService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            IProjectMapper projectMapper,
            ITaskMapper taskMapper)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _projectMapper = projectMapper;
            _taskMapper = taskMapper;
        }

        public IEnumerable<ProjectListDTO> GetAllProjects()
        {
            var projectDataModels = _projectRepository.GetProjects();

            // Для кожного проєкту рахуємо статистику та перетворюємо в DTO
            foreach (var project in projectDataModels)
            {
                var tasks = _taskRepository.GetTasksByProjectId(project.Id).ToList();

                var totalTasks = tasks.Count;
                var completedTasks = tasks.Count(t => t.IsCompleted);

                yield return _projectMapper.MapToListDTO(project, totalTasks, completedTasks);
            }
        }

        public ProjectDetailsDTO? GetProjectById(Guid id)
        {
            var projectData = _projectRepository.GetProjectById(id);
            if (projectData == null) return null;

            // Отримуємо DataModel тасок через репозиторій і відразу конвертуємо їх у DTO
            var tasksData = _taskRepository.GetTasksByProjectId(id);
            var tasksDto = tasksData.Select(t => _taskMapper.MapToListDTO(t)).ToList();

            return _projectMapper.MapToDetailsDTO(projectData, tasksDto);
        }
    }
}