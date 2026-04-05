using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Common; // Обов'язково для EnumExtensions
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq; // Для FirstOrDefault
using System.Threading.Tasks;

namespace KMA.TaskManager.Maui.ViewModels
{
    public partial class TaskEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITaskService _taskService;
        private Guid _taskId;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        // Масив для Picker
        [ObservableProperty]
        private EnumWithName<TaskPriority>[] _priorities;

        // Вибране значення для Picker
        [ObservableProperty]
        private EnumWithName<TaskPriority> _selectedPriority;

        [ObservableProperty]
        private DateTime _dueDate;

        [ObservableProperty]
        private bool _isCompleted;

        public TaskEditViewModel(ITaskService taskService)
        {
            _taskService = taskService;

            // Завантажуємо всі можливі варіанти при створенні ViewModel
            Priorities = EnumExtensions.GetValuesWithNames<TaskPriority>();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("TaskId") && query["TaskId"] is Guid taskId)
            {
                _taskId = taskId;
            }
        }

        public async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                var task = await _taskService.GetTaskByIdAsync(_taskId)
                    ?? throw new Exception("Завдання не знайдено.");

                Name = task.Name;
                Description = task.Description;

                SelectedPriority = Priorities.FirstOrDefault(p => p.Value == task.Priority);

                DueDate = task.DueDate.DateTime;
                IsCompleted = task.IsCompleted;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Помилка", ex.Message, "OK");
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Валідація", "Назва не може бути порожньою", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                var editModel = new TaskEditModel(
                    _taskId,
                    Name,
                    Description,
                    SelectedPriority.Value,
                    DueDate,
                    IsCompleted);

                await _taskService.UpdateTaskAsync(editModel);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Помилка", $"Не вдалося оновити завдання: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Помилка", $"Не вдалося повернутися назад: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}