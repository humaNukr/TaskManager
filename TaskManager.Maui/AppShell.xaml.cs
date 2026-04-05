using KMA.TaskManager.Maui.Pages;

namespace KMA.TaskManager.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // ProjectDetails реєструємо як прямий нащадок головної сторінки
            Routing.RegisterRoute("ProjectDetails", typeof(ProjectDetails));

            // TaskDetails реєструємо як вкладений маршрут відносно деталей проєкту
            Routing.RegisterRoute("ProjectDetails/TaskDetails", typeof(TaskDetails));

            Routing.RegisterRoute("ProjectCreatePage", typeof(ProjectCreatePage));
            Routing.RegisterRoute("ProjectEditPage", typeof(Pages.ProjectEditPage));
        }
    }
}
