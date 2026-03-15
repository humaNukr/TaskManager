using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.Storage;
using KMA.TaskManager.UIModels;
using System;
using System.Threading.Tasks;

namespace KMA.TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskMapper _taskMapper;

        // Впровадження залежності через конструктор(Constructor Injection)
        public TaskService(ITaskRepository taskRepository, ITaskMapper taskMapper)
        {
            _taskRepository = taskRepository;
            _taskMapper = taskMapper;
        }

        //Отримання завдань за ідентифікатором проекту
        public IEnumerable<TaskListDto> GetTasksByProjectId(Guid projectId)
        {
            // Отримуємо тільки ті завдання, що належать конкретному проєкту
            var tasks = _taskRepository.GetTasksByProjectId(projectId);

            // Мапимо кожну DataModel у DTO для передачі даних у UI
            foreach (var task in tasks)
            {
                yield return _taskMapper.MapToListDTO(task);
            }
        }

        //Детальна Інформація про завдання
        public TaskDetailsDto? GetTaskById(Guid taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            //Мапимо DataModel у DTO для передачі даних у UI
            return _taskMapper.MapToDetailsDTO(task);
        }
    }
}