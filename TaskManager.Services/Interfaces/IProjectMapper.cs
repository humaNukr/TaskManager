using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface IProjectMapper
    {
        ProjectUIModel MapToUI(ProjectDataModel data, int total, int completed);
        ProjectDataModel MapToData(ProjectCreateModel model);
    }
}