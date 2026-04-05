using System.ComponentModel.DataAnnotations;

namespace KMA.TaskManager.Common.Enums
{
    public enum TaskPriority
    {
        [Display(Name = "Низький")]
        Low,
        [Display(Name = "Середній")]
        Medium,
        [Display(Name = "Високий")]
        High,
        [Display(Name = "Критичний")]
        Critical
    }
}