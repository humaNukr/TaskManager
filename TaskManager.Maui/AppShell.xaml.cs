using KMA.TaskManager.Maui.Pages;

namespace KMA.TaskManager.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Реєструємо маршрути проектів
            Routing.RegisterRoute("ProjectDetails", typeof(ProjectDetails));
            Routing.RegisterRoute("ProjectCreatePage", typeof(ProjectCreatePage));
            Routing.RegisterRoute("ProjectEditPage", typeof(Pages.ProjectEditPage));

            // Реєструємо маршрути завдань
            Routing.RegisterRoute("TaskCreatePage", typeof(TaskCreatePage));
            Routing.RegisterRoute("TaskDetails", typeof(TaskDetails));
            Routing.RegisterRoute("TaskEditPage", typeof(TaskEditPage));
        }
    }
}