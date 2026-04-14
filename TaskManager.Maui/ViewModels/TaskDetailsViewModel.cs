using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.Services.DTOModels.Tasks;

namespace KMA.TaskManager.Maui.ViewModels
{
    public partial class TaskDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ITaskService _taskService;
        private Guid _taskId;

        [ObservableProperty]
        private TaskDetailsDto _currentTask;

        public TaskDetailsViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("TaskId", out var taskIdObj))
            {
                if (taskIdObj is Guid guidId)
                {
                    _taskId = guidId;
                }
                else if (taskIdObj is string stringId && Guid.TryParse(stringId, out Guid parsedId))
                {
                    _taskId = parsedId;
                }
            }
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                CurrentTask = await _taskService.GetTaskByIdAsync(_taskId)
                    ?? throw new Exception("Завдання не існує у системі.");
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
        private async Task DeleteTaskAsync()
        {
            IsBusy = true;
            try
            {
                bool confirm = await Shell.Current.DisplayAlert("Підтвердження", "Ви дійсно хочете видалити це завдання?", "Так", "Ні");

                if (confirm)
                {
                    var result = await _taskService.DeleteTaskAsync(_taskId);
                    if (result)
                        await Shell.Current.GoToAsync("..");
                    else
                        throw new Exception("Не вдалося видалити завдання з бази даних.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Помилка", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task EditTaskAsync()
        {
            await Shell.Current.GoToAsync($"TaskEditPage", new Dictionary<string, object>
            {
                { "TaskId", _taskId }
            });
        }
    }
}