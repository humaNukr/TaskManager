using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages;

public partial class TaskDetails : ContentPage
{
    public TaskDetails(TaskDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}