using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Maui.ViewModels;

[QueryProperty(nameof(ProjectId), "ProjectId")]
public partial class ProjectEditViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;

    [ObservableProperty]
    private Guid _projectId;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private ProjectType _selectedProjectType;

    public ProjectType[] ProjectTypes { get; } = (ProjectType[])Enum.GetValues(typeof(ProjectType));

    public ProjectEditViewModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    async partial void OnProjectIdChanged(Guid value)
    {
        await LoadProjectAsync(value);
    }

    private async Task LoadProjectAsync(Guid id)
    {
        IsBusy = true;
        try
        {
            var project = await _projectService.GetProjectDetailsAsync(id);
            if (project != null)
            {
                Name = project.Name;
                Description = project.Description;
                SelectedProjectType = project.ProjectType;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Помилка", "Назва не може бути порожньою", "ОК");
            return;
        }

        IsBusy = true;
        try
        {
            var editModel = new ProjectEditModel(ProjectId, Name, Description, SelectedProjectType);
            await _projectService.UpdateProjectAsync(editModel);
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}