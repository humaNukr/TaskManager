using System.Collections.Generic;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Services.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        public ProjectListDTO MapToListDTO(ProjectDataModel data, int total, int completed)
        {
            if (data == null) return null;

            return new ProjectListDTO(
                data.Id,
                data.Name,
                total,
                completed
            );
        }

        public ProjectDetailsDTO MapToDetailsDTO(ProjectDataModel data, IEnumerable<TaskListDTO> tasks)
        {
            if (data == null) return null;

            return new ProjectDetailsDTO(
                data.Id,
                data.Name,
                data.Description,
                data.ProjectType,
                tasks
            );
        }

        public ProjectDataModel MapToData(ProjectCreateModel model)
        {
            if (model == null) return null;
            return new ProjectDataModel(
                model.Name,
                model.Description,
                model.ProjectType
            );
        }
    }
}