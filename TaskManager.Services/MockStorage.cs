using System;
using System.Collections.Generic;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;

namespace KMA.TaskManager.Services
{
    internal static class MockStorage
    {
        private static readonly List<TaskDataModel> _tasks;
        private static readonly List<ProjectDataModel> _projects;
        internal static IEnumerable<TaskDataModel> Tasks => _tasks.ToList();
        internal static IEnumerable<ProjectDataModel> Projects => _projects.ToList();

        static MockStorage()
        {
            _projects = new List<ProjectDataModel>
            {
                new ProjectDataModel("Розробка вебсайту пекарні", "Створення сайту-візитки з каталогом продукції для місцевої кондитерської \"Зефір\"", ProjectType.Work),
                new ProjectDataModel("Курс програмування на C#", "Виконання лабораторних та практичних робіт у рамках першого семестру", ProjectType.Educational),
                new ProjectDataModel("Ремонт у вітальні", "Планування бюджету, вибір матеріалів та пошук майстрів для оновлення інтер'єру", ProjectType.Personal),
                new ProjectDataModel("Дослідження ринку ШІ", "Аналіз сучасних трендів у сфері штучного інтелекту для написання наукової статті", ProjectType.Research)
            };
        }
    }
}
