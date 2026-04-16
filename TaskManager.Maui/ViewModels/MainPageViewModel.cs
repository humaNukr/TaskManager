using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Maui.Controls;

namespace KMA.TaskManager.Maui.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;

    private IEnumerable<ProjectListDTO> _allProjects = Enumerable.Empty<ProjectListDTO>();

    [ObservableProperty]
    private ObservableCollection<ProjectListDTO> _projects = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedSortOption = "За назвою (А-Я)";

    [ObservableProperty]
    private string _selectedStatusFilter = "Усі проєкти";

    public string[] SortOptions { get; } = { "За назвою (А-Я)", "За назвою (Я-А)", "За прогресом (спадання)", "За прогресом (зростання)" };

    public string[] StatusFilters { get; } = { "Усі проєкти", "В процесі", "Завершені" };

    public MainPageViewModel(IProjectService projectService)
    {
        _projectService = projectService;
        _ = InitializeAsync();
    }

    partial void OnSearchTextChanged(string value) => ApplyFiltersAndSorting();
    partial void OnSelectedSortOptionChanged(string value) => ApplyFiltersAndSorting();
    partial void OnSelectedStatusFilterChanged(string value) => ApplyFiltersAndSorting();

    [RelayCommand]
    private async Task InitializeAsync()
    {
        if (_allProjects != null && _allProjects.Any())
        {
            try
            {
                _allProjects = await _projectService.GetAllProjectsAsync();
                ApplyFiltersAndSorting();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Помилка", $"Не вдалося завантажити проєкти: {ex.Message}", "ОК");
            }
            return;
        }

        IsBusy = true;
        try
        {
            await Task.Delay(300);
            _allProjects = await _projectService.GetAllProjectsAsync();
            ApplyFiltersAndSorting();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося завантажити проєкти: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LoadProjectsAsync()
    {
        IsBusy = true;
        try
        {
            _allProjects = await _projectService.GetAllProjectsAsync();
            ApplyFiltersAndSorting();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося оновити список: {ex.Message}", "ОК");
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

        if (SelectedStatusFilter == "Завершені")
        {
            filtered = filtered.Where(p => p.Progress >= 100);
        }
        else if (SelectedStatusFilter == "В процесі")
        {
            filtered = filtered.Where(p => p.Progress < 100);
        }

        filtered = SelectedSortOption switch
        {
            "За назвою (Я-А)" => filtered.OrderByDescending(p => p.Name),
            "За прогресом (спадання)" => filtered.OrderByDescending(p => p.Progress),
            "За прогресом (зростання)" => filtered.OrderBy(p => p.Progress),
            _ => filtered.OrderBy(p => p.Name)
        };

        Projects = new ObservableCollection<ProjectListDTO>(filtered);
    }

    [RelayCommand]
    private async Task DeleteProjectAsync(Guid id)
    {
        try
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Видалення",
                "Ви впевнені? Усі пов'язані завдання також будуть видалені.",
                "Так", "Ні");

            if (!confirm) return;

            IsBusy = true;
            bool result = await _projectService.DeleteProjectAsync(id);
            if (result)
            {
                // Завантажуємо оновлений список безпосередньо тут
                _allProjects = await _projectService.GetAllProjectsAsync();
                ApplyFiltersAndSorting();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Не вдалося видалити проєкт: {ex.Message}", "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("ProjectCreatePage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync(Guid id)
    {
        try
        {
            var navParams = new Dictionary<string, object> { { "ProjectId", id } };
            await Shell.Current.GoToAsync("ProjectEditPage", navParams);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(Guid id)
    {
        try
        {
            var navParams = new Dictionary<string, object> { { "ProjectId", id } };
            await Shell.Current.GoToAsync("ProjectDetails", navParams);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка навігації", ex.Message, "ОК");
        }
    }
}