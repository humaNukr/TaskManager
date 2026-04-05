using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskService _taskService;

    public ProjectService(IProjectRepository projectRepository, ITaskService taskService)
    {
        _projectRepository = projectRepository;
        _taskService = taskService;
    }

    public async Task<IEnumerable<ProjectListDTO>> GetAllProjectsAsync()
    {
        var projectsData = await _projectRepository.GetAllProjectsAsync();
        var resultList = new List<ProjectListDTO>();

        foreach (var p in projectsData)
        {
            // беремо таски щоб порахувати прогрес проєкту для списку
            var tasks = await _taskService.GetTasksByProjectIdAsync(p.Id);
            int totalTasks = tasks.Count();
            int completedTasks = tasks.Count(t => t.IsCompleted);

            resultList.Add(new ProjectListDTO(p.Id, p.Name, totalTasks, completedTasks));
        }

        return resultList;
    }

    public async Task<ProjectDetailsDTO> GetProjectDetailsAsync(Guid id)
    {
        var projectData = await _projectRepository.GetProjectByIdAsync(id);
        if (projectData == null) return null;

        var tasks = await _taskService.GetTasksByProjectIdAsync(id);

        return new ProjectDetailsDTO(
            projectData.Id,
            projectData.Name,
            projectData.Description,
            projectData.ProjectType,
            tasks
        );
    }

    public async Task<ProjectDetailsDTO> CreateProjectAsync(ProjectCreateModel createModel)
    {
        var dataModel = new ProjectDataModel(createModel.Name, createModel.Description, createModel.ProjectType);
        var saved = await _projectRepository.SaveProjectAsync(dataModel);

        return await GetProjectDetailsAsync(saved.Id);
    }

    public async Task<ProjectDetailsDTO> UpdateProjectAsync(ProjectEditModel editModel)
    {
        var existing = await _projectRepository.GetProjectByIdAsync(editModel.Id);
        if (existing == null) throw new ArgumentException("Проєкт не знайдено.");

        existing.Name = editModel.Name;
        existing.Description = editModel.Description;
        existing.ProjectType = editModel.ProjectType;

        await _projectRepository.SaveProjectAsync(existing);

        return await GetProjectDetailsAsync(existing.Id);
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        // каскадне видалення тасок, які належать цьому проєкту
        await _taskService.DeleteTasksByProjectIdAsync(id);

        // видалення самого проєкту
        return await _projectRepository.DeleteProjectAsync(id);
    }
}