using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Maui.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;

        [ObservableProperty]
        private ObservableCollection<ProjectListDTO> _projects;

        public MainPageViewModel(IProjectService projectService)
        {
            _projectService = projectService;
            LoadProjects();
        }

        private void LoadProjects()
        {
            var projectsFromService = _projectService.GetAllProjects();
            Projects = new ObservableCollection<ProjectListDTO>(projectsFromService);
        }

        [RelayCommand]
        private async Task OpenProjectDetails(Guid projectId)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "ProjectId", projectId }
            };

            await Shell.Current.GoToAsync("ProjectDetails", navigationParameter);
        }
    }
}