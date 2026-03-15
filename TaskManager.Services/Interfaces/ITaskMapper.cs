using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface ITaskMapper
    {
        TaskUIModel MapToUI(TaskDataModel data);
        TaskDataModel MapToData(TaskCreateModel model);
        TaskListDto MapToListDTO(TaskDataModel data);
        TaskDetailsDto MapToDetailsDTO(TaskDataModel data);
    }
}