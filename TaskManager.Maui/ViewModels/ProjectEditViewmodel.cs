using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Maui.Controls;

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

    // Правило активації кнопки збереження
    public bool CanSave => !string.IsNullOrWhiteSpace(Name) && !IsBusy;

    partial void OnNameChanged(string value)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(IsBusy))
        {
            SaveCommand.NotifyCanExecuteChanged();
        }
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
            else
            {
                await Shell.Current.DisplayAlert("Помилка", "Проєкт не знайдено.", "ОК");
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося завантажити проєкт: {ex.Message}", "ОК");
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        IsBusy = true;
        try
        {
            var editModel = new ProjectEditModel(ProjectId, Name, Description, SelectedProjectType);
            await _projectService.UpdateProjectAsync(editModel);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося оновити проєкт: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
    }
}