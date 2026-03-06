using System;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.Storage;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly IStorageContext _storageContext;
        private readonly ITaskMapper _taskMapper;

        // Впровадження залежності через конструктор(Constructor Injection)
        public TaskService(IStorageContext storageContext, ITaskMapper taskMapper)
        {
            _storageContext = storageContext;
            _taskMapper = taskMapper;
        }

        //Отримання завдань за ідентифікатором проекту
        public List<TaskUIModel> GetTasksByProjectId(Guid projectId)
        {
            // Отримуємо тільки ті завдання, що належать конкретному проєкту
            var tasksData = _storageContext.GetTasksByProjectId(projectId);

            // Мапимо кожну DataModel у UIModel для коректного відображення в списку
            return tasksData.Select(_taskMapper.MapToUI).ToList();
        }

        //Детальна Інформація про завдання
        public TaskUIModel? GetTaskById(Guid id)
        {
            var taskData = _storageContext.GetTaskById(id);
            return taskData != null ? _taskMapper.MapToUI(taskData) : null;
        }
    }
}