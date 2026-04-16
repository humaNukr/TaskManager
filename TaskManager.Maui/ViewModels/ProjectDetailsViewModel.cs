using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace KMA.TaskManager.Maui.ViewModels;

[QueryProperty(nameof(ProjectId), "ProjectId")]
public partial class ProjectDetailsViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;

    [ObservableProperty]
    private Guid _projectId;

    [ObservableProperty]
    private ProjectDetailsDTO? _currentProject;

    [ObservableProperty]
    private ObservableCollection<TaskListDTO> _displayedTasks = new();

    [ObservableProperty]
    private string _taskSearchText = string.Empty;

    [ObservableProperty]
    private string _selectedTaskFilter = "Усі завдання";

    public string[] TaskFilters { get; } = {
        "Усі завдання",
        "Тільки активні",
        "Тільки завершені",
        "Критичні спочатку",
        "Найближчий дедлайн"
    };

    public ProjectDetailsViewModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    async partial void OnProjectIdChanged(Guid value)
    {
        await LoadProjectDetailsAsync(value);
    }

    partial void OnTaskSearchTextChanged(string value) => ApplyTaskFilters();
    partial void OnSelectedTaskFilterChanged(string value) => ApplyTaskFilters();

    [RelayCommand]
    private async Task LoadProjectDetailsAsync(Guid id)
    {
        IsBusy = true;
        try
        {
            CurrentProject = await _projectService.GetProjectDetailsAsync(id);
            ApplyTaskFilters();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося завантажити деталі проєкту: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyTaskFilters()
    {
        if (CurrentProject?.Tasks == null) return;

        var filtered = CurrentProject.Tasks.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(TaskSearchText))
        {
            filtered = filtered.Where(t => t.Name.Contains(TaskSearchText, StringComparison.OrdinalIgnoreCase));
        }

        filtered = SelectedTaskFilter switch
        {
            "Тільки активні" => filtered.Where(t => !t.IsCompleted),
            "Тільки завершені" => filtered.Where(t => t.IsCompleted),
            "Критичні спочатку" => filtered.OrderByDescending(t => t.Priority),
            "Найближчий дедлайн" => filtered.Where(t => !t.IsCompleted).OrderBy(t => t.DueDate),
            _ => filtered
        };

        var resultList = filtered.ToList();

        Application.Current?.Dispatcher.Dispatch(() =>
        {
            DisplayedTasks.Clear();
            foreach (var task in resultList)
            {
                DisplayedTasks.Add(task);
            }
        });
    }

    [RelayCommand]
    private async Task OpenTaskDetailsAsync(Guid taskId)
    {
        try
        {
            await Shell.Current.GoToAsync($"TaskDetails?TaskId={taskId}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task CreateTaskAsync()
    {
        try
        {
            await Shell.Current.GoToAsync($"TaskCreatePage?ProjectId={ProjectId}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
    }
}