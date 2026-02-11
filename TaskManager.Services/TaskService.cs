using System;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services
{
    public class TaskService
    {
        public TaskUIModel MapToUI(TaskDataModel data)
        {
            return data is null
                ? throw new ArgumentNullException(nameof(data))
                : new TaskUIModel(
                    data.Id,
                    data.ProjectId,
                    data.Name,
                    data.Description,
                    data.Priority,
                    data.DueDate,
                    data.IsCompleted
                );
        }
    }
}