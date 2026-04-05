using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDataModel>> GetTasksAsync();
        Task<IEnumerable<TaskDataModel>> GetTasksByProjectIdAsync(Guid projectId);
        Task<TaskDataModel> GetTaskByIdAsync(Guid id);
        Task<TaskDataModel> SaveTaskAsync(TaskDataModel task);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<bool> DeleteTasksByProjectIdAsync(Guid projectId);
    }
}