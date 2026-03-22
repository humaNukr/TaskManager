using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Maui.ViewModels;

namespace KMA.TaskManager.Maui.Pages;
public partial class TaskDetails : ContentPage
{
    public TaskDetails(TaskDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}