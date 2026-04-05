using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Storage;

public interface IStorageContext
{
    Task<IEnumerable<ProjectDataModel>> GetProjectsAsync();
    Task<ProjectDataModel> GetProjectByIdAsync(Guid id);
    Task<ProjectDataModel> SaveProjectAsync(ProjectDataModel project);
    Task<bool> DeleteProjectAsync(Guid id);

    Task<IEnumerable<TaskDataModel>> GetTasksAsync();
    Task<IEnumerable<TaskDataModel>> GetTasksByProjectIdAsync(Guid projectId);
    Task<TaskDataModel> GetTaskByIdAsync(Guid id);
    Task<TaskDataModel> SaveTaskAsync(TaskDataModel task);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<bool> DeleteTasksByProjectIdAsync(Guid projectId);
}