using System;

namespace KMA.TaskManager.Services.DTOModels.Projects;

public class ProjectListDTO
{
    public Guid Id { get; }
    public string Name { get; }
    public int TotalTasks { get; }
    public int CompletedTasks { get; }
    public double Progress { get; }

    public ProjectListDTO(
        Guid id,
        string name,
        int totalTasks,
        int completedTasks,
        double progress)
    {
        Id = id;
        Name = name;
        TotalTasks = totalTasks;
        CompletedTasks = completedTasks;
        Progress = progress;
    }
}