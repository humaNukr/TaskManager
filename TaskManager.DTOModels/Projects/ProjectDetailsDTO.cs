using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public IReadOnlyCollection<TaskListDTO> Tasks { get; }

    public int TotalTasks { get; }
    public int CompletedTasks { get; }
    public double ProgressFraction { get; }
    public string ProgressStats { get; }

    public ProjectDetailsDTO(
        Guid id,
        string name,
        string description,
        ProjectType projectType,
        IEnumerable<TaskListDTO> tasks,
        int totalTasks,
        int completedTasks,
        double progressFraction,
        string progressStats)
    {
        Id = id;
        Name = name;
        Description = description;
        ProjectType = projectType;
        Tasks = tasks?.ToList().AsReadOnly() ?? new ReadOnlyCollection<TaskListDTO>(new List<TaskListDTO>());
        TotalTasks = totalTasks;
        CompletedTasks = completedTasks;
        ProgressFraction = progressFraction;
        ProgressStats = progressStats;
    }
}