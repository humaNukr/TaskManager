using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.CreateModels
{
    public class TaskCreateModel
    {
        public Guid ProjectId { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskPriority Priority { get; }
        public DateTimeOffset DueDate { get; }

        public TaskCreateModel(
            Guid projectId,
            string name,
            string description,
            TaskPriority priority,
            DateTimeOffset dueDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва завдання не може бути порожньою", nameof(name));

            ProjectId = projectId;
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
        }
    }
}
