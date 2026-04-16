using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using System.Collections.Generic;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.DTOModels.Tasks;

namespace KMA.TaskManager.Services.Interfaces
{
    public interface IProjectMapper
    {
        ProjectListDTO MapToListDTO(ProjectDataModel data, int total, int completed);
        ProjectDetailsDTO MapToDetailsDTO(ProjectDataModel data, IEnumerable<TaskListDTO> tasks);
        ProjectDataModel MapToData(ProjectCreateModel model);
    }
}