using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Services.DTOModels.Tasks
{
    public class TaskDetailsDto
    {
        public Guid Id { get; }
        public Guid ProjectId { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskPriority Priority { get; }
        public DateTimeOffset DueDate { get; }
        public bool IsCompleted { get; }
        public bool IsOverdue { get; }

        public TaskDetailsDto(
            Guid id,
            Guid projectId,
            string name,
            string description,
            TaskPriority priority,
            DateTimeOffset dueDate,
            bool isCompleted,
            bool isOverdue)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            IsCompleted = isCompleted;
            IsOverdue = isOverdue;
        }
    }
}