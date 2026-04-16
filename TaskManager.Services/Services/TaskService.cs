using KMA.TaskManager.CreateModels;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KMA.TaskManager.DTOModels.Tasks;

namespace KMA.TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskMapper _taskMapper;

        public TaskService(ITaskRepository taskRepository, ITaskMapper taskMapper)
        {
            _taskRepository = taskRepository;
            _taskMapper = taskMapper;
        }

        public async Task<IEnumerable<TaskListDTO>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);

            // Захист: повертаємо порожню колекцію замість null, щоб уникнути NullReferenceException
            if (tasks == null || !tasks.Any())
            {
                return Enumerable.Empty<TaskListDTO>();
            }

            return tasks.Select(t => _taskMapper.MapToListDTO(t)).ToList();
        }

        public async Task<TaskDetailsDto?> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);

            if (task == null) return null;

            return _taskMapper.MapToDetailsDTO(task);
        }

        public async Task<TaskDetailsDto?> CreateTaskAsync(TaskCreateModel createModel)
        {
            var dataModel = _taskMapper.MapToData(createModel);

            var savedTask = await _taskRepository.SaveTaskAsync(dataModel);
            return _taskMapper.MapToDetailsDTO(savedTask);
        }

        public async Task<TaskDetailsDto?> UpdateTaskAsync(TaskEditModel editModel)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(editModel.Id);

            // Кидаємо виняток замість тихого повернення null
            if (existingTask == null)
                throw new ArgumentException($"Завдання з ID {editModel.Id} не знайдено.");

            _taskMapper.MapUpdateToData(editModel, existingTask);

            var updatedTask = await _taskRepository.SaveTaskAsync(existingTask);

            return _taskMapper.MapToDetailsDTO(updatedTask);
        }

        public async Task<bool> DeleteTasksByProjectIdAsync(Guid projectId)
        {
            return await _taskRepository.DeleteTasksByProjectIdAsync(projectId);
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId)
        {
            return await _taskRepository.DeleteTaskAsync(taskId);
        }
    }
}