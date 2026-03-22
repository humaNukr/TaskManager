using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Storage;

namespace KMA.TaskManager.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IStorageContext _storage;

    public TaskRepository(IStorageContext storage)
    {
        _storage = storage;
    }

    public TaskDataModel? GetTaskById(Guid id)
    {
        return _storage.GetTaskById(id);
    }

    public IEnumerable<TaskDataModel> GetTasksByProjectId(Guid projectId)
    {
        return _storage.GetTasksByProjectId(projectId);
    }
}