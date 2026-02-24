using KMA.TaskManager.UIModels;
namespace KMA.TaskManager.Maui.Pages;

public partial class ProjectDetails : ContentPage
{
    public ProjectDetails(ProjectUIModel project)
    {
        InitializeComponent();

        BindingContext = new
        {
            project.Name,
            project.ProjectType,
            project.Description,
            project.Progress,
            ProgressFraction = project.Progress / 100.0,
            ProgressStats = $"({project.CompletedTasksCount} з {project.TotalTasksCount} завершено)"
        };
    }
}