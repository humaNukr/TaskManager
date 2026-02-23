using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;

namespace TaskManager.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly IProjectService _projectService;

        public MainPage(IProjectService projectService)
        {
            InitializeComponent();
            _projectService = projectService;
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
