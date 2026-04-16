using System.ComponentModel.DataAnnotations;

namespace KMA.TaskManager.Common.Enums;

public enum ProjectType
{
    [Display(Name = "Навчальний")]
    Educational,
    [Display(Name = "Робочий")]
    Work,
    [Display(Name = "Особистий")]
    Personal,
    [Display(Name = "Дослідницький")]
    Research,
    [Display(Name = "Хобі")]
    Hobby,
    [Display(Name = "Волонтерський")]
    Volunteer,
    [Display(Name = "Інше")]
    Other
}