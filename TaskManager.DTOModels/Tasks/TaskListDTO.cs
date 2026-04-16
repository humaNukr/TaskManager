using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.DTOModels.Tasks
{
    public class TaskListDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public TaskPriority Priority { get; }
        public bool IsCompleted { get; }
        public bool IsOverdue { get; }
        public DateTime DueDate { get; }

        public TaskListDTO(
            Guid id,
            string name,
            TaskPriority priority,
            bool isCompleted,
            bool isOverdue,
            DateTime dueDate)
        {
            Id = id;
            Name = name;
            Priority = priority;
            IsCompleted = isCompleted;
            IsOverdue = isOverdue;
            DueDate = dueDate;
        }
    }
}