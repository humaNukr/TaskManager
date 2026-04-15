using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Common; // Для EnumExtensions та EnumWithName
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.Services.Interfaces;
using System.Collections.ObjectModel;

namespace KMA.TaskManager.Maui.ViewModels
{
    public partial class TaskCreateViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITaskService _taskService;
        private Guid _projectId;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        // Список всіх варіантів пріоритету з красивими назвами для Picker
        [ObservableProperty]
        private EnumWithName<TaskPriority>[] _priorities;

        // Обраний користувачем пріоритет (об'єкт, що містить і Enum, і назву)
        [ObservableProperty]
        private EnumWithName<TaskPriority> _selectedPriority;

        // Поле дати
        [ObservableProperty]
        private DateTime _dueDate = DateTime.Now.AddDays(1);

        public TaskCreateViewModel(ITaskService taskService)
        {
            _taskService = taskService;

            Priorities = EnumExtensions.GetValuesWithNames<TaskPriority>();

            SelectedPriority = Priorities.FirstOrDefault(p => p.Value == TaskPriority.Medium);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("ProjectId", out var projectIdObj))
            { if (projectIdObj is Guid guidId)
                {
                    _projectId = guidId;
                }
                else if (projectIdObj is string stringId && Guid.TryParse(stringId, out Guid parsedId))
                {
                    _projectId = parsedId;
                }
            }
        }

        [RelayCommand]
        private async Task SaveTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Валідація", "Назва завдання обов'язкова", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                // Використовуємо SelectedPriority.Value, щоб дістати чистий Enum для моделі
                var createModel = new TaskCreateModel(
                    _projectId,
                    Name,
                    Description,
                    SelectedPriority.Value,
                    DueDate);

                await _taskService.CreateTaskAsync(createModel);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Помилка", $"Не вдалося створити завдання: {ex.Message}", "OK");
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