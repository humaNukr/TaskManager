using System;

namespace KMA.TaskManager.Services.DTOModels.Projects;
public record ProjectListDTO(
    Guid Id,
    string Name,
    int TotalTasks,
    int CompletedTasks
)
{
    public double Progress => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks * 100;
}