using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KMA.TaskManager.UIModels;

public class TaskUIModel
{
    // id is read-only
    public Guid Id { get; }

    public Guid ProjectId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsOverdue => !IsCompleted && DueDate < DateTimeOffset.Now;


    public TaskUIModel
    (
        Guid id,
        Guid projectId,
        string name,
        string description,
        TaskPriority priority,
        DateTimeOffset dueDate,
        bool isCompleted
    )
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }

    public override string ToString()
    {
        string statusText;
        if (IsCompleted)
            statusText = "✅ Виконано";
        else if (IsOverdue)
            statusText = "❌ ПРОСТРОЧЕНО";
        else
            statusText = "⏳ В процесі";

        return $"------------------------------------------\n" +
               $"Завдання:  {Name}\n" +
               $"Статус:    {statusText}\n" +
               $"Пріоритет: {Priority}\n" +
               $"Термін до: {DueDate:dd.MM.yyyy HH:mm}\n" +
               $"Опис:      {Description}\n" +
               $"------------------------------------------";
    }
}