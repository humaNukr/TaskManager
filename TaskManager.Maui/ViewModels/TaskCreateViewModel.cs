using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Common;
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
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private EnumWithName<TaskPriority>[] _priorities = Array.Empty<EnumWithName<TaskPriority>>();

        [ObservableProperty]
        private EnumWithName<TaskPriority> _selectedPriority = default!;

        [ObservableProperty]
        private DateTime _dueDate = DateTime.Now.AddDays(1);

        public TaskCreateViewModel(ITaskService taskService)
        {
            _taskService = taskService;

            Priorities = EnumExtensions.GetValuesWithNames<TaskPriority>();

            SelectedPriority = Priorities.FirstOrDefault(p => p.Value == TaskPriority.Medium) ?? Priorities[0];
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("ProjectId", out var projectIdObj))
            {
                if (projectIdObj is Guid guidId)
                {
                    _projectId = guidId;
                }
                else if (projectIdObj is string stringId && Guid.TryParse(stringId, out Guid parsedId))
                {
                    _projectId = parsedId;
                }
            }
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(Name) && !IsBusy;

        partial void OnNameChanged(string value)
        {
            SaveTaskCommand.NotifyCanExecuteChanged();
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(IsBusy))
            {
                SaveTaskCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveTaskAsync()
        {
            IsBusy = true;
            try
            {
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