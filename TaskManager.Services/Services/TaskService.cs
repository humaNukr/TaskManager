using KMA.TaskManager.CreateModels;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KMA.TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskMapper _taskMapper;

        // Впровадження залежності через конструктор
        public TaskService(ITaskRepository taskRepository, ITaskMapper taskMapper)
        {
            _taskRepository = taskRepository;
            _taskMapper = taskMapper;
        }

        // Отримання завдань за ідентифікатором проєкту
        public async Task<IEnumerable<TaskListDTO>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);

            // Мапінг кожної DataModel у DTO для передачі даних у UI за допомогою LINQ
            return tasks.Select(t => _taskMapper.MapToListDTO(t)).ToList();
        }

        // Детальна інформація про завдання
        public async Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);

            if (task == null) return null;

            // Мапінг DataModel у DTO для передачі даних у UI
            return _taskMapper.MapToDetailsDTO(task);
        }

        // Створення нового завдання
        public async Task<TaskDetailsDto> CreateTaskAsync(TaskCreateModel createModel)
        {
            var dataModel = _taskMapper.MapToData(createModel);

            var savedTask = await _taskRepository.SaveTaskAsync(dataModel);

            // 3. Повертаємо DTO, щоб UI міг відразу показати створену таску
            return _taskMapper.MapToDetailsDTO(savedTask);
        }

        // Оновлення існуючого завдання
        public async Task<TaskDetailsDto?> UpdateTaskAsync(TaskEditModel editModel)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(editModel.Id);
            if (existingTask == null)
                return null;

            _taskMapper.MapUpdateToData(editModel, existingTask);

            var updatedTask = await _taskRepository.SaveTaskAsync(existingTask);

            return _taskMapper.MapToDetailsDTO(updatedTask);
        }

        // Видалення завдань за ідентифікатором проєкту
        public async Task<bool> DeleteTasksByProjectIdAsync(Guid projectId)
        {
            // Передаємо команду в репозиторій
            return await _taskRepository.DeleteTasksByProjectIdAsync(projectId);
        }

        // Видалення завдання
        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            return await _taskRepository.DeleteTaskAsync(taskId);
        }
    }
}