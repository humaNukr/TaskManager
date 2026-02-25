using KMA.TaskManager.Maui.Pages;

namespace KMA.TaskManager.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectDetails), typeof(ProjectDetails));
            Routing.RegisterRoute(nameof(TaskDetails), typeof(TaskDetails));
        }
    }
}
