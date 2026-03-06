using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Storage
{
    public interface IStorageContext
    {
        IEnumerable<ProjectDataModel> GetProjects();
        IEnumerable<TaskDataModel> GetTasksByProjectId(Guid projectId);
        ProjectDataModel? GetProjectById(Guid id);
        TaskDataModel? GetTaskById(Guid id);
        int GetTasksCountByProjectId(Guid projectId);
    }
}