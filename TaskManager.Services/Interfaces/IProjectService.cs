using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Services.DTOModels.Projects;

namespace KMA.TaskManager.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectListDTO>> GetAllProjectsAsync();
    Task<ProjectDetailsDTO> GetProjectDetailsAsync(Guid id);
    Task<ProjectDetailsDTO> CreateProjectAsync(ProjectCreateModel createModel);
    Task<ProjectDetailsDTO> UpdateProjectAsync(ProjectEditModel editModel);
    Task<bool> DeleteProjectAsync(Guid id);
}