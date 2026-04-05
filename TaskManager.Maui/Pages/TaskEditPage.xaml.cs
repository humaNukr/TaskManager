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

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (BindingContext is TaskEditViewModel vm)
            {
                await vm.RefreshData();
            }
        }
    }
}