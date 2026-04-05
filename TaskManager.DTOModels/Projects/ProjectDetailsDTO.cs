using System;
using System.Collections.Generic;
using System.Linq;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Services.DTOModels.Projects;

public class ProjectDetailsDTO
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ProjectType ProjectType { get; }
    public IEnumerable<TaskListDTO> Tasks { get; }

    public ProjectDetailsDTO(
        Guid id,
        string name,
        string description,
        ProjectType projectType,
        IEnumerable<TaskListDTO> tasks)
    {
        Id = id;
        Name = name;
        Description = description;
        ProjectType = projectType;
        Tasks = tasks;
    }

    public int TotalTasks => Tasks?.Count() ?? 0;
    public int CompletedTasks => Tasks?.Count(t => t.IsCompleted) ?? 0;
    public double ProgressFraction => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks;
    public double Progress => ProgressFraction * 100;

    public string ProgressStats => $"{CompletedTasks} з {TotalTasks} завдань завершено";
}