using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<ProjectDataModel>> GetAllProjectsAsync();
    Task<ProjectDataModel> GetProjectByIdAsync(Guid id);
    Task<ProjectDataModel> SaveProjectAsync(ProjectDataModel project);
    Task<bool> DeleteProjectAsync(Guid id);
}