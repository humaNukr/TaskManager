using Microsoft.Maui.Controls;
using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages;

public partial class ProjectCreatePage : ContentPage
{
    public ProjectCreatePage(ProjectCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}