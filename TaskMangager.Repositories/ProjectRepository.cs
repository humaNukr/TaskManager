using System;
using System.Collections.Generic;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Storage;

namespace KMA.TaskManager.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IStorageContext _storage;

    public ProjectRepository(IStorageContext storage)
    {
        _storage = storage;
    }

    public IEnumerable<ProjectDataModel> GetProjects()
    {
        return _storage.GetProjects();
    }

    public ProjectDataModel? GetProjectById(Guid id)
    {
        return _storage.GetProjectById(id);
    }
}