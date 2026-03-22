using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.DTOModels.Tasks;
using System.Collections.Generic;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface IProjectMapper
    {
        ProjectListDTO MapToListDTO(ProjectDataModel data, int total, int completed);
        ProjectDetailsDTO MapToDetailsDTO(ProjectDataModel data, IEnumerable<TaskListDTO> tasks);
        ProjectDataModel MapToData(ProjectCreateModel model);
    }
}