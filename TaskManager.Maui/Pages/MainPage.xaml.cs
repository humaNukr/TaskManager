using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}