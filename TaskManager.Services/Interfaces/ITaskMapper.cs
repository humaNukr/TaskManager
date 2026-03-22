using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface ITaskMapper
    {
        TaskDataModel MapToData(TaskCreateModel model);
        TaskListDTO MapToListDTO(TaskDataModel data);
        TaskDetailsDto MapToDetailsDTO(TaskDataModel data);
    }
}