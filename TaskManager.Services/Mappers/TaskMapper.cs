using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.TaskManager.Services.Mappers
{
    internal static class TaskMapper
    {
        // Метод забезпечує дотримання принципу Single Responsibility: 
        // він відповідає виключно за створення UI-проекції на основі даних із "бази".
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
