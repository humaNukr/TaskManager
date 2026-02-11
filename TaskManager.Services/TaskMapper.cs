using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.TaskManager.Services
{
    internal static class TaskMapper
    {
        public static TaskUIModel MapToUI(TaskDataModel data)
        {
            if (data == null) return null;

            return new TaskUIModel(
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
