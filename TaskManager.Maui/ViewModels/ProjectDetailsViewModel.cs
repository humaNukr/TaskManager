using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Maui.ViewModels
{
    public partial class ProjectDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IProjectService _projectService;

        [ObservableProperty]
        private ProjectDetailsDTO _currentProject;

        public ProjectDetailsViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("ProjectId") && query["ProjectId"] is Guid projectId)
            {
                // Отримуємо деталі проєкту разом з тасками через сервіс
                CurrentProject = _projectService.GetProjectById(projectId);
            }
        }

        [RelayCommand]
        private void OpenTaskDetails(Guid taskId)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "TaskId", taskId }
            };

            Shell.Current.GoToAsync("TaskDetails", navigationParameter);
        }
    }
}