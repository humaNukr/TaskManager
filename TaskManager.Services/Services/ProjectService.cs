using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskService _taskService;
    private readonly IProjectMapper _projectMapper;

    public ProjectService(IProjectRepository projectRepository, ITaskService taskService, IProjectMapper projectMapper)
    {
        _projectRepository = projectRepository;
        _taskService = taskService;
        _projectMapper = projectMapper;
    }

    public async Task<IEnumerable<ProjectListDTO>> GetAllProjectsAsync()
    {
        var projectsData = await _projectRepository.GetAllProjectsAsync();

        // Архітектурний захист: метод, що повертає колекцію, ніколи не повинен повертати null
        if (projectsData == null || !projectsData.Any())
        {
            return Enumerable.Empty<ProjectListDTO>();
        }

        var resultList = new List<ProjectListDTO>();

        foreach (var p in projectsData)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(p.Id);

            int totalTasks = tasks?.Count() ?? 0;
            int completedTasks = tasks?.Count(t => t.IsCompleted) ?? 0;

            var dto = _projectMapper.MapToListDTO(p, totalTasks, completedTasks);
            resultList.Add(dto);
        }

        return resultList;
    }

    public async Task<ProjectDetailsDTO?> GetProjectDetailsAsync(Guid id)
    {
        var projectData = await _projectRepository.GetProjectByIdAsync(id);
        if (projectData == null) return null;

        var tasks = await _taskService.GetTasksByProjectIdAsync(id);

        return _projectMapper.MapToDetailsDTO(projectData, tasks);
    }

    public async Task<ProjectDetailsDTO?> CreateProjectAsync(ProjectCreateModel createModel)
    {
        var dataModel = _projectMapper.MapToData(createModel);

        var saved = await _projectRepository.SaveProjectAsync(dataModel);

        return await GetProjectDetailsAsync(saved.Id);
    }

    public async Task<ProjectDetailsDTO?> UpdateProjectAsync(ProjectEditModel editModel)
    {
        var existing = await _projectRepository.GetProjectByIdAsync(editModel.Id);

        if (existing == null) throw new ArgumentException($"Проєкт з ID {editModel.Id} не знайдено.");

        existing.Name = editModel.Name;
        existing.Description = editModel.Description;
        existing.ProjectType = editModel.ProjectType;

        await _projectRepository.SaveProjectAsync(existing);

        return await GetProjectDetailsAsync(existing.Id);
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        await _taskService.DeleteTasksByProjectIdAsync(id);
        return await _projectRepository.DeleteProjectAsync(id);
    }
}