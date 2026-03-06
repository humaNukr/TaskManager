using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.CreateModels
{
    /// <summary>
    /// Ця модель використовується коли користувач заповнює форму створення нової таски
    /// Полів isOverdue та isCompleted тут немає, оскільки при створенні нової таски вона завжди буде невиконаною та не простроченою.
    /// </summary>
    public class TaskCreateModel
    {
        public Guid ProjectId { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskPriority Priority { get; }
        public DateTimeOffset DueDate { get; }

        // Конструктор вимагає всі необхідні поля
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
