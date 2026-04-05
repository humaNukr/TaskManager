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

            Routing.RegisterRoute("ProjectDetails/TaskCreate", typeof(TaskCreatePage));

            // Деталі таски та її редагування
            Routing.RegisterRoute("ProjectDetails/TaskDetails", typeof(TaskDetails));
            Routing.RegisterRoute("ProjectDetails/TaskDetails/TaskEdit", typeof(TaskEditPage));
        }
    }
}
