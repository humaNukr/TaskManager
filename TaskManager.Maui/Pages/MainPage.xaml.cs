using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Maui.Pages;

public partial class MainPage : ContentPage
{
    private readonly IProjectService _projectService;

    private List<ProjectUIModel> _projects;

    public List<ProjectUIModel> Projects
    {
        get => _projects;
        set
        {
            _projects = value;
            BindingContext = _projects;
        }
    }

    public MainPage(IProjectService projectService)
    {
        InitializeComponent();
        _projectService = projectService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Projects = _projectService.GetAllProjects();
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