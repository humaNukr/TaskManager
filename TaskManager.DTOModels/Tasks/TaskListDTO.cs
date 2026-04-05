using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Services.DTOModels.Tasks
{
    public class TaskListDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public TaskPriority Priority { get; }
        public bool IsCompleted { get; }
        public bool IsOverdue { get; }

        public TaskListDTO(
            Guid id,
            string name,
            TaskPriority priority,
            bool isCompleted,
            bool isOverdue)
        {
            Id = id;
            Name = name;
            Priority = priority;
            IsCompleted = isCompleted;
            IsOverdue = isOverdue;
        }
    }
}