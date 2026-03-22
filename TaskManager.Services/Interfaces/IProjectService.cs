using System;
using System.Collections.Generic;
using KMA.TaskManager.Services.DTOModels.Projects;

namespace KMA.TaskManager.Services.Interfaces;

public interface IProjectService
{
    IEnumerable<ProjectListDTO> GetAllProjects();
    ProjectDetailsDTO? GetProjectById(Guid projectId);
}