using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services.Mappers
{
    internal static class ProjectMapper
    {
        public static ProjectUIModel MapToUI(ProjectDataModel data, int total, int completed)
        {
            if (data == null) return null;

            return new ProjectUIModel(
                data.Id,
                data.Name,
                data.Description,
                data.ProjectType,
                total,
                completed
            );
        }
    }
}
