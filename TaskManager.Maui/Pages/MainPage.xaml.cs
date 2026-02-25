using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Maui.Pages;

public partial class MainPage : ContentPage
{
    private readonly IProjectService _projectService;

    public MainPage(IProjectService projectService)
    {
        InitializeComponent();
        _projectService = projectService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var projects = _projectService.GetAllProjects();

        BindingContext = projects;
    }

    private async void OnProjectSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ProjectUIModel selectedProject)
        {
            await DisplayAlert("Тест", "Клік спрацював!", "ОК");
            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedProject", selectedProject }
            };

            await Shell.Current.GoToAsync(nameof(ProjectDetails), navigationParameter);
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnProjectTapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is ProjectUIModel selectedProject)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedProject", selectedProject }
            };
            await Shell.Current.GoToAsync(nameof(ProjectDetails), navigationParameter);
        }
    }
}