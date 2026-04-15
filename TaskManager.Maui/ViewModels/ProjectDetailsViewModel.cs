using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace KMA.TaskManager.Maui.ViewModels;

// Використовуємо QueryProperty для отримання ID проекту з параметрів навігації
[QueryProperty(nameof(ProjectId), "ProjectId")]
public partial class ProjectDetailsViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;

    [ObservableProperty]
    private Guid _projectId;

    [ObservableProperty]
    private ProjectDetailsDTO _currentProject;

    public ProjectDetailsViewModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // Метод викликається автоматично при зміні ProjectId (після навігації)
    async partial void OnProjectIdChanged(Guid value)
    {
        await LoadProjectDetailsAsync(value);
    }

    [RelayCommand]
    private async Task LoadProjectDetailsAsync(Guid id)
    {
        IsBusy = true; // Використовуємо IsBusy з BaseViewModel для керування індикатором завантаження
        try
        {
            await Task.Delay(300);
            // Викликаємо оновлений асинхронний метод сервісу
            CurrentProject = await _projectService.GetProjectDetailsAsync(id);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenTaskDetailsAsync(Guid taskId)
    {
        // Асинхронна навігація до деталей завдання
        await Shell.Current.GoToAsync($"TaskDetails?TaskId={taskId}");
    }

    [RelayCommand]
    private async Task CreateTaskAsync()
    {
        // Перехід на сторінку створення завдання для поточного проекту
        await Shell.Current.GoToAsync($"TaskCreatePage?ProjectId={ProjectId}");
    }
}