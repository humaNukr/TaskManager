using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages
{
    public partial class TaskCreatePage : ContentPage
    {
        public TaskCreatePage(TaskCreateViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}