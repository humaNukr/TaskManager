using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Storage;

namespace KMA.TaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IStorageContext _storage;

        public TaskRepository(IStorageContext storage)
        {
            _storage = storage;
        }

        public Task<IEnumerable<TaskDataModel>> GetTasksAsync()
        {
            return _storage.GetTasksAsync();
        }

        public Task<IEnumerable<TaskDataModel>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return _storage.GetTasksByProjectIdAsync(projectId);
        }

        public Task<TaskDataModel> GetTaskByIdAsync(Guid id)
        {
            return _storage.GetTaskByIdAsync(id);
        }

        public Task<TaskDataModel> SaveTaskAsync(TaskDataModel task)
        {
            return _storage.SaveTaskAsync(task);
        }

        public Task<bool> DeleteTaskAsync(Guid id)
        {
            return _storage.DeleteTaskAsync(id);
        }

        public Task<bool> DeleteTasksByProjectIdAsync(Guid projectId)
        {
            return _storage.DeleteTasksByProjectIdAsync(projectId);
        }
    }
}