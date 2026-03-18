using System;
using System.Collections.Generic;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Services.DTOModels.Projects;

public record ProjectDetailsDTO(
    Guid Id,
    string Name,
    string Description,
    ProjectType ProjectType,
    IEnumerable<TaskListDto> Tasks
);