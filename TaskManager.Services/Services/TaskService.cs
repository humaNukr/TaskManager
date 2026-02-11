using System;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services
{
    public class TaskService
    {
        //Отримання завдань за ідентифікатором проекту
        public List<TaskUIModel> GetTasksByProjectId(Guid projectId)
        {
            return MockStorage.Tasks
                .Where(t => t.ProjectId == projectId)
                .Select(TaskMapper.MapToUI)
                .ToList();
        }

        //Детальна Інформація про завдання
        public TaskUIModel? GetTaskById(Guid taskId)
        {
            var task = MockStorage.Tasks.FirstOrDefault(t => t.Id == taskId);
            return task != null ? TaskMapper.MapToUI(task) : null;
        }






    }
}