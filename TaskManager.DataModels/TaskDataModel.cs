using KMA.TaskManager.Common.Enums;
using SQLite;

namespace KMA.TaskManager.DataModels
{
    public class TaskDataModel
    {
        // Додаємо атрибут PrimaryKey, щоб SQLite розумів, що це унікальний ідентифікатор
        [PrimaryKey] public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public TaskDataModel()
        {
        }

        public TaskDataModel(Guid projectId, string name, string description, TaskPriority priority,
            DateTimeOffset dueDate, bool isCompleted)
            : this(Guid.NewGuid(), projectId, name, description, priority, dueDate, isCompleted)
        {
        }

        public TaskDataModel(Guid id, Guid projectId, string name, string description,
            TaskPriority priority, DateTimeOffset dueDate, bool isCompleted)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            IsCompleted = isCompleted;
        }
    }
}