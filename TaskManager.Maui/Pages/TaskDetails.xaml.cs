using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Maui.ViewModels;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Maui.Pages;

[QueryProperty(nameof(Task), "SelectedTask")]
public partial class TaskDetails : ContentPage
{
    public TaskDetails(TaskDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}