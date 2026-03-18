using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        //Отримання завдань за ідентифікатором проекту
        IEnumerable<TaskListDTO> GetTasksByProjectId(Guid projectId);

        //Детальна Інформація про завдання
        TaskDetailsDto? GetTaskById(Guid taskId);
    }
}
