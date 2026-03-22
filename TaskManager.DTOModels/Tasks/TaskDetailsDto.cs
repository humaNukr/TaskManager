using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Services.DTOModels.Tasks;

public record TaskDetailsDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string Description,
    TaskPriority Priority,
    DateTimeOffset DueDate,
    bool IsCompleted,
    bool IsOverdue
);