using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services.Mappers
{
    public class TaskMapper : ITaskMapper
    {
        // Метод забезпечує дотримання принципу Single Responsibility: 
        // він відповідає виключно за створення UI-проекції на основі даних із "бази".
        public TaskUIModel MapToUI(TaskDataModel data)
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

        public TaskDataModel MapToData(TaskCreateModel model)
        {
            if (model == null) return null;

            // при створенні таска завжди IsCompleted = false
            return new TaskDataModel(
                model.ProjectId,
                model.Name,
                model.Description,
                model.Priority,
                model.DueDate,
                false
            );
        }
    }
}
