using KMA.TaskManager.Maui.Pages;
using KMA.TaskManager.Maui.ViewModels;
using KMA.TaskManager.Repositories;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.Services.Mappers;
using KMA.TaskManager.Services.Services;
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

            //Реєстрація залежностей в IoC-контейнері для забезпечення слабкої зв'язності
            //між компонентами застосунку

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // сторінки реєструємо як Transient — новий екземпляр при кожному переході
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProjectDetails>();
            builder.Services.AddTransient<TaskDetails>();
            builder.Services.AddTransient<TaskEditPage>();
            builder.Services.AddTransient<TaskCreatePage>();
            builder.Services.AddTransient<ProjectCreatePage>();
            builder.Services.AddTransient<ProjectEditPage>();

            builder.Services.AddTransient<TaskDetailsViewModel>();
            builder.Services.AddTransient<TaskEditViewModel>();
            builder.Services.AddTransient<TaskCreateViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ProjectDetailsViewModel>();
            builder.Services.AddTransient<ProjectCreateViewModel>();
            builder.Services.AddTransient<ProjectEditViewModel>();

            // сервіси — Singleton, бо сховище спільне для всього застосунку
            builder.Services.AddSingleton<IProjectMapper, ProjectMapper>();
            builder.Services.AddSingleton<ITaskMapper, TaskMapper>();

            builder.Services.AddSingleton<IStorageContext, SQLLiteStorageContext>();

            builder.Services.AddSingleton<ITaskService, TaskService>();
            builder.Services.AddSingleton<IProjectService, ProjectService>();

            builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
            builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();

            return builder.Build();
        }
    }
}