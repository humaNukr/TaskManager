using System;

namespace KMA.TaskManager.Services.DTOModels.Projects;
public record ProjectListDTO(
    Guid Id,
    string Name,
    int TotalTasks,
    int CompletedTasks
);