using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainPageViewModel vm)
        {
            vm.LoadProjectsCommand.Execute(null);
        }
    }
}