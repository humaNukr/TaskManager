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
            var bakeryWebsite = new ProjectDataModel("Розробка вебсайту пекарні",
                "Створення сайту-візитки з каталогом продукції для місцевої кондитерської \"Зефір\"", ProjectType.Work);
            var csharpCourse = new ProjectDataModel("Курс програмування на C#",
                "Виконання лабораторних та практичних робіт у рамках першого семестру", ProjectType.Educational);
            var homeRenovation = new ProjectDataModel("Ремонт у вітальні",
                "Планування бюджету, вибір матеріалів та пошук майстрів для оновлення інтер'єру", ProjectType.Personal);
            var aiMarketResearch = new ProjectDataModel("Дослідження ринку ШІ",
                "Аналіз сучасних трендів у сфері штучного інтелекту для написання наукової статті",
                ProjectType.Research);

            _projects = new List<ProjectDataModel>
            {
                bakeryWebsite,
                csharpCourse,
                homeRenovation,
                aiMarketResearch
            };

            _tasks = new List<TaskDataModel>
            {
                new TaskDataModel(bakeryWebsite.Id, "Аналіз вимог", "Зустріч з власником для ТЗ", TaskPriority.High, DateTimeOffset.Now.AddDays(2), true),
                new TaskDataModel(bakeryWebsite.Id, "Макет дизайну", "Розробка стилю в Figma", TaskPriority.Medium, DateTimeOffset.Now.AddDays(5), false),
                new TaskDataModel(bakeryWebsite.Id, "Верстка головної", "HTML/CSS адаптивна верстка", TaskPriority.High, DateTimeOffset.Now.AddDays(7), false),
                new TaskDataModel(bakeryWebsite.Id, "Каталог товарів", "Розробка сторінки з випічкою", TaskPriority.Medium, DateTimeOffset.Now.AddDays(10), false),
                new TaskDataModel(bakeryWebsite.Id, "Форма замовлення", "Логіка відправки запитів на email", TaskPriority.High, DateTimeOffset.Now.AddDays(12), false),
                new TaskDataModel(bakeryWebsite.Id, "Налаштування SEO", "Оптимізація мета-тегів", TaskPriority.Low, DateTimeOffset.Now.AddDays(15), false),
                new TaskDataModel(bakeryWebsite.Id, "Тестування", "Перевірка кросбраузерності", TaskPriority.High, DateTimeOffset.Now.AddDays(16), false),
                new TaskDataModel(bakeryWebsite.Id, "Наповнення текстами", "Копірайтинг для розділу про нас", TaskPriority.Low, DateTimeOffset.Now.AddDays(18), false),
                new TaskDataModel(bakeryWebsite.Id, "Фотосесія", "Зйомка десертів для каталогу", TaskPriority.Medium, DateTimeOffset.Now.AddDays(20), false),
                new TaskDataModel(bakeryWebsite.Id, "Деплой", "Перенесення сайту на хостинг", TaskPriority.Critical, DateTimeOffset.Now.AddDays(21), false),

                new TaskDataModel(csharpCourse.Id, "Лабораторна 1", "Реалізація моделей та сервісів", TaskPriority.High, DateTimeOffset.Now.AddDays(3), true),
                new TaskDataModel(csharpCourse.Id, "Лабораторна 2", "Робота з GUI та подіями", TaskPriority.High, DateTimeOffset.Now.AddDays(14), false)
            };
        }
    }
}
