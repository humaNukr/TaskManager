using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface ITaskMapper
    {
        TaskUIModel MapToUI(TaskDataModel data);
        TaskDataModel MapToData(TaskCreateModel model);
    }
}