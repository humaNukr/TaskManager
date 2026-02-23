using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using TaskManager.Maui.Pages;

namespace TaskManager.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IProjectService, ProjectService>();
            builder.Services.AddSingleton<ITaskService, TaskService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProjectDetailsPage>();
            builder.Services.AddTransient<TaskDetailsPage>();

            return builder.Build();
        }
    }
}
