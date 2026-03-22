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
    IEnumerable<TaskListDTO> Tasks
)
{
    public int TotalTasks => Tasks?.Count() ?? 0;
    public int CompletedTasks => Tasks?.Count(t => t.IsCompleted) ?? 0;

    public double Progress => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks * 100;
    public double ProgressFraction => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks;
    public string ProgressStats => $"{CompletedTasks} з {TotalTasks} завдань завершено";
}