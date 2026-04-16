using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace KMA.TaskManager.Maui.ViewModels;

public partial class ProjectCreateViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private ProjectType _selectedProjectType;

    public ProjectType[] ProjectTypes { get; } = (ProjectType[])Enum.GetValues(typeof(ProjectType));

    public ProjectCreateViewModel(IProjectService projectService)
    {
        _projectService = projectService;
        SelectedProjectType = ProjectType.Personal;
    }

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

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        IsBusy = true;
        try
        {
            var createModel = new ProjectCreateModel(Name, Description, SelectedProjectType);
            await _projectService.CreateProjectAsync(createModel);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося створити проєкт: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        IsBusy = true;
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }
}