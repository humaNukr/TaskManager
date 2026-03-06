using KMA.TaskManager.Maui.Pages;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.Storage;
using Microsoft.Extensions.Logging;

namespace KMA.TaskManager.Maui
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
            // сторінки реєструємо як Transient — новий екземпляр при кожному переході
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProjectDetails>();
            builder.Services.AddTransient<TaskDetails>();

            // сервіси — Singleton, бо сховище спільне для всього застосунку
            builder.Services.AddSingleton<IProjectMapper, ProjectMapper>();
            builder.Services.AddSingleton<ITaskMapper, TaskMapper>();

            builder.Services.AddSingleton<IStorageContext, InMemoryStorageContext>();

            builder.Services.AddSingleton<ITaskService, TaskService>();
            builder.Services.AddSingleton<IProjectService, ProjectService>();

            return builder.Build();
        }
    }
}
