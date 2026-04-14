using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace KMA.TaskManager.Maui.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;

    // Повний список проектів для фільтрації в пам'яті
    private IEnumerable<ProjectListDTO> _allProjects;

    [ObservableProperty]
    private ObservableCollection<ProjectListDTO> _projects = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedSortOption = "За назвою (А-Я)";

    public string[] SortOptions { get; } = { "За назвою (А-Я)", "За прогресом (спадання)" };

    public MainPageViewModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // Автоматично викликається при зміні SearchText
    partial void OnSearchTextChanged(string value) => ApplyFiltersAndSorting();

    // Автоматично викликається при зміні сортування
    partial void OnSelectedSortOptionChanged(string value) => ApplyFiltersAndSorting();

    [RelayCommand]
    private async Task LoadProjectsAsync()
    {
        IsBusy = true;
        try
        {
            _allProjects = await _projectService.GetAllProjectsAsync();
            ApplyFiltersAndSorting();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyFiltersAndSorting()
    {
        if (_allProjects == null) return;

        var filtered = _allProjects;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedSortOption == "За прогресом (спадання)")
        {
            filtered = filtered.OrderByDescending(p => p.Progress);
        }
        else
        {
            filtered = filtered.OrderBy(p => p.Name);
        }

        Projects = new ObservableCollection<ProjectListDTO>(filtered);
    }

    [RelayCommand]
    private async Task DeleteProjectAsync(Guid id)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Видалення",
            "Ви впевнені? Усі пов'язані завдання також будуть видалені.",
            "Так", "Ні");

        if (!confirm) return;

        bool result = await _projectService.DeleteProjectAsync(id);
        if (result)
        {
            await LoadProjectsAsync();
        }
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
        => await Shell.Current.GoToAsync("ProjectCreatePage");

    [RelayCommand]
    private async Task GoToEditAsync(Guid id)
    {
        var navParams = new Dictionary<string, object>
        {
            { "ProjectId", id }
        };
        await Shell.Current.GoToAsync("ProjectEditPage", navParams);
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Guid id)
    {
        var navParams = new Dictionary<string, object>
        {
            { "ProjectId", id }
        };
        await Shell.Current.GoToAsync("ProjectDetails", navParams);
    }
}