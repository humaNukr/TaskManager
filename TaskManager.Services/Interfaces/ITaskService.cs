using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskListDTO>> GetTasksByProjectIdAsync(Guid projectId);

        Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId);

        Task<TaskDetailsDto> CreateTaskAsync(TaskCreateModel createModel);

        Task<TaskDetailsDto?> UpdateTaskAsync(TaskEditModel editModel);

        Task<bool> DeleteTasksByProjectIdAsync(Guid projectId);

        Task<bool> DeleteTaskAsync(Guid taskId);
    }
}