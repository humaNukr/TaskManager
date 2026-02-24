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
            await Navigation.PushAsync(new ProjectDetails(selectedProject));
        }

        ((CollectionView)sender).SelectedItem = null;
    }
}