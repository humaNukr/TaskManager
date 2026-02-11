using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Services
{
    public class ProjectService
    {
        public List<ProjectUIModel> GetAllProjects()
        {
            return MockStorage.Projects
                .Select(p =>
                {
                    var total = MockStorage.Tasks.Count(t => t.ProjectId == p.Id);
                    var completed = MockStorage.Tasks.Count(t => t.ProjectId == p.Id && t.IsCompleted);
                    return ProjectMapper.MapToUI(p, total, completed);
                })
                .ToList();
        }

        public ProjectUIModel? GetProjectById(Guid projectId)
        {
            var project = MockStorage.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null) return null;
            var total = MockStorage.Tasks.Count(t => t.ProjectId == project.Id);
            var completed = MockStorage.Tasks.Count(t => t.ProjectId == project.Id && t.IsCompleted);
            return ProjectMapper.MapToUI(project, total, completed);
        }
    }
}
