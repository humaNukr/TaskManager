using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.UIModels;
namespace KMA.TaskManager.Maui.Pages;

[QueryProperty(nameof(Project), "SelectedProject")]
public partial class ProjectDetails : ContentPage
{
    private readonly ITaskService _taskService;

    private ProjectUIModel _project;
    public ProjectUIModel Project
    {
        get => _project;
        set
        {
            _project = value;
            OnPropertyChanged();
            UpdateBindingContext();

            LoadTasks();
        }
    }

    public ProjectDetails(ITaskService taskService)
    {
        InitializeComponent();
        _taskService = taskService;
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

    private void LoadTasks()
    {
        if (Project == null) return;

        var tasks = _taskService.GetTasksByProjectId(Project.Id);
        TasksListView.ItemsSource = tasks;
    }

    private async void OnTaskTapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is TaskUIModel selectedTask)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedTask", selectedTask }
            };
            await Shell.Current.GoToAsync(nameof(TaskDetails), navigationParameter);
        }
    }
}