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
        // Отримання списку завдань для проєкту
        Task<IEnumerable<TaskListDTO>> GetTasksByProjectIdAsync(Guid projectId);

        // Детальна інформація про завдання
        Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId);

        // Створення нового завдання
        Task<TaskDetailsDto> CreateTaskAsync(TaskCreateModel createModel);

        // Редагування існуючого завдання
        Task<TaskDetailsDto?> UpdateTaskAsync(TaskEditModel editModel);

        // Видалення всіх завдань за ідентифікатором проєкту
        Task<bool> DeleteTasksByProjectIdAsync(Guid projectId);

        // Видалення завдання
        Task<bool> DeleteTaskAsync(Guid taskId);
    }
}