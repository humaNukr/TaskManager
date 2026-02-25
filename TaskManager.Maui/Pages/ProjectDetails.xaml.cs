using KMA.TaskManager.UIModels;
namespace KMA.TaskManager.Maui.Pages;

[QueryProperty(nameof(Project), "SelectedProject")]
public partial class ProjectDetails : ContentPage
{
    private ProjectUIModel _project;
    public ProjectUIModel Project
    {
        get => _project;
        set
        {
            _project = value;
            OnPropertyChanged();
            UpdateBindingContext();
        }
    }

    public ProjectDetails()
    {
        InitializeComponent();
    }

    private void UpdateBindingContext()
    {
        if (Project == null) return;

        BindingContext = new
        {
            Project.Name,
            Project.ProjectType,
            Project.Description,
            Project.Progress,
            ProgressFraction = Project.Progress / 100.0,
            ProgressStats = $"({Project.CompletedTasksCount} з {Project.TotalTasksCount} завершено)"
        };
    }
}