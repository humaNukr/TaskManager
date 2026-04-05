namespace KMA.TaskManager.Maui.Pages;

public partial class ProjectEditPage : ContentPage
{
    public ProjectEditPage(ViewModels.ProjectEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}