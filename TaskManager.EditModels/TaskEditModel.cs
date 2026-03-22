using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.EditModels;

public class TaskEditModel
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public TaskPriority Priority { get; }
    public DateTimeOffset DueDate { get; }
    public bool IsCompleted { get; }

    public TaskEditModel(
        Guid id,
        string name,
        string description,
        TaskPriority priority,
        DateTimeOffset dueDate,
        bool isCompleted)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва не може бути порожньою", nameof(name));

        Id = id;
        Name = name;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }
}