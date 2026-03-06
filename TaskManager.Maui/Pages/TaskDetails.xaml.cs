using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Maui.Pages;

[QueryProperty(nameof(Task), "SelectedTask")]
public partial class TaskDetails : ContentPage
{
    private TaskUIModel _task;

    public TaskUIModel Task
    {
        get => _task;
        set
        {
            _task = value;
            BindingContext = _task;
        }
    }

    public TaskDetails()
    {
        InitializeComponent();
    }
}
