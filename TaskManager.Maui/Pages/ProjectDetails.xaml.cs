using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages;
public partial class ProjectDetails : ContentPage
{
    public ProjectDetails(ProjectDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProjectDetailsViewModel vm)
        {
            if (vm.ProjectId != Guid.Empty)
            {
                vm.LoadProjectDetailsCommand.Execute(vm.ProjectId);
            }
        }
    }
}