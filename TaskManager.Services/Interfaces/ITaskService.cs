using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        //Отримання завдань за ідентифікатором проекту
        List<TaskUIModel> GetTasksByProjectId(Guid projectId);

        //Детальна Інформація про завдання
        TaskUIModel? GetTaskById(Guid taskId);
    }
}
