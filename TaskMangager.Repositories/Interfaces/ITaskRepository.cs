using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Repositories.Interfaces;

public interface ITaskRepository
{
    IEnumerable<TaskDataModel> GetTasksByProjectId(Guid projectId);
    TaskDataModel? GetTaskById(Guid id);
}