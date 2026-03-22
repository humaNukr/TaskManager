using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Maui.ViewModels
{
    // Наслідуємо ObservableObject для підтримки Binding
    public partial class TaskDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        private TaskDetailsDto _currentTask;

        public TaskDetailsViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("TaskId") && query["TaskId"] is Guid taskId)
            {
                // Отримуємо деталі таски через сервіс
                CurrentTask = _taskService.GetTaskById(taskId);
            }
        }
    }
}