using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Storage;

namespace KMA.TaskManager.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IStorageContext _storageContext;

    public ProjectRepository(IStorageContext storageContext)
    {
        _storageContext = storageContext ?? throw new ArgumentNullException(nameof(storageContext));
    }

    public Task<IEnumerable<ProjectDataModel>> GetAllProjectsAsync() => _storageContext.GetProjectsAsync();

    public Task<ProjectDataModel> GetProjectByIdAsync(Guid id) => _storageContext.GetProjectByIdAsync(id);

    public Task<ProjectDataModel> SaveProjectAsync(ProjectDataModel project) => _storageContext.SaveProjectAsync(project);

    public Task<bool> DeleteProjectAsync(Guid id) => _storageContext.DeleteProjectAsync(id);
}