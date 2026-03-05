using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services.Mappers
{
    public class ProjectMapper : IProjectMapper
    {
        // total і completed передаються ззовні, бо DataModel не знає про завдання —
        // це відповідальність сервісного шару
        public ProjectUIModel MapToUI(ProjectDataModel data, int total, int completed)
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

        public ProjectDataModel MapToData(ProjectCreateModel model)
        {
            if (model == null) return null;

            // створюємо нову модель даних (Id згенерується автоматично в конструкторі DataModel)
            return new ProjectDataModel(
                model.Name,
                model.Description,
                model.ProjectType
            );
        }
    }
}
