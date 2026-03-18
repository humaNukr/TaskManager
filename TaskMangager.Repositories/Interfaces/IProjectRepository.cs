using System;
using System.Collections.Generic;
using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Repositories.Interfaces;

public interface IProjectRepository
{
    IEnumerable<ProjectDataModel> GetProjects();
    ProjectDataModel? GetProjectById(Guid id);
}