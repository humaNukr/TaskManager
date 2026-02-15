using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.DataModels
{
    public class TaskDataModel
    {
        // id is read-only
        public Guid Id { get; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public TaskDataModel(Guid projectId, string name, string description, TaskPriority priority,
            DateTimeOffset dueDate, bool isCompleted)
        {
            Id = Guid.NewGuid();
            ProjectId = projectId;
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            IsCompleted = isCompleted;
        }
    }
}
