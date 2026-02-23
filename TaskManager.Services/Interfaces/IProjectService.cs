using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services.Interfaces;

public interface IProjectService
{
    List<ProjectUIModel> GetAllProjects();
    ProjectUIModel? GetProjectById(Guid projectId);
}