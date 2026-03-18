using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Services.DTOModels.Tasks;

public record TaskListDTO(
    Guid Id,
    string Name,
    TaskPriority Priority,
    bool IsCompleted,
    bool IsOverdue
);