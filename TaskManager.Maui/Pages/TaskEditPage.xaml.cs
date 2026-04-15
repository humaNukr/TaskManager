using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages
{
    public partial class TaskEditPage : ContentPage
    {
        public TaskEditPage(TaskEditViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}