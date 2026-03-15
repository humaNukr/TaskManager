using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Storage.Interfaces;

namespace KMA.TaskManager.Storage
{
    public class InMemoryStorageContext : IStorageContext
    {
        private record ProjectRecord(Guid Id, string Name, string Description, ProjectType ProjectType);
        private record TaskRecord(Guid Id, Guid ProjectId, string Name, string Description, TaskPriority Priority, DateTimeOffset DueDate, bool IsCompleted);

        private static readonly List<ProjectRecord> _projects = new List<ProjectRecord>();
        private static readonly List<TaskRecord> _tasks = new List<TaskRecord>();

        #region MockStoragePopulation
        static InMemoryStorageContext()
        {
            var bakeryWebsite = new ProjectRecord(Guid.NewGuid(), "Розробка вебсайту пекарні",
                "Створення сайту-візитки з каталогом продукції для місцевої кондитерської \"Зефір\"", ProjectType.Work);
            var csharpCourse = new ProjectRecord(Guid.NewGuid(), "Курс програмування на C#",
                "Виконання лабораторних та практичних робіт у рамках першого семестру", ProjectType.Educational);
            var homeRenovation = new ProjectRecord(Guid.NewGuid(), "Ремонт у вітальні",
                "Планування бюджету, вибір матеріалів та пошук майстрів для оновлення інтер'єру", ProjectType.Personal);
            var aiMarketResearch = new ProjectRecord(Guid.NewGuid(), "Дослідження ринку ШІ",
                "Аналіз сучасних трендів у сфері штучного інтелекту для написання наукової статті", ProjectType.Research);

            _projects.Add(bakeryWebsite);
            _projects.Add(csharpCourse);
            _projects.Add(homeRenovation);
            _projects.Add(aiMarketResearch);

            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Аналіз вимог", "Зустріч з власником для ТЗ", TaskPriority.High, DateTimeOffset.Now.AddDays(1), true));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Макет дизайну", "Розробка стилю в Figma", TaskPriority.Medium, DateTimeOffset.Now.AddDays(5), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Верстка головної", "HTML/CSS адаптивна верстка", TaskPriority.High, DateTimeOffset.Now.AddDays(7), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Каталог товарів", "Розробка сторінки з випічкою", TaskPriority.Medium, DateTimeOffset.Now.AddDays(10), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Форма замовлення", "Логіка відправки запитів на email", TaskPriority.High, DateTimeOffset.Now.AddDays(12), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Налаштування SEO", "Оптимізація мета-тегів", TaskPriority.Low, DateTimeOffset.Now.AddDays(15), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Тестування", "Перевірка кросбраузерності", TaskPriority.High, DateTimeOffset.Now.AddDays(16), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Наповнення текстами", "Копірайтинг для розділу про нас", TaskPriority.Low, DateTimeOffset.Now.AddDays(-2), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Фотосесія", "Зйомка десертів для каталогу", TaskPriority.Medium, DateTimeOffset.Now.AddDays(20), false));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), bakeryWebsite.Id, "Деплой", "Перенесення сайту на хостинг", TaskPriority.Critical, DateTimeOffset.Now.AddDays(21), false));

            _tasks.Add(new TaskRecord(Guid.NewGuid(), csharpCourse.Id, "Лабораторна 1", "Реалізація моделей та сервісів", TaskPriority.High, DateTimeOffset.Now.AddDays(3), true));
            _tasks.Add(new TaskRecord(Guid.NewGuid(), csharpCourse.Id, "Лабораторна 2", "Робота з GUI та подіями", TaskPriority.High, DateTimeOffset.Now.AddDays(14), false));
        }
        #endregion

        public IEnumerable<ProjectDataModel> GetProjects()
        {
            foreach (var project in _projects)
            {
                yield return new ProjectDataModel(project.Id, project.Name, project.Description, project.ProjectType);
            }
        }

        public ProjectDataModel? GetProjectById(Guid id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            return project is null ? null : new ProjectDataModel(project.Id, project.Name, project.Description, project.ProjectType);
        }

        public TaskDataModel? GetTaskById(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return task is null ? null : new TaskDataModel(task.Id, task.ProjectId, task.Name, task.Description, task.Priority, task.DueDate, task.IsCompleted);
        }

        public IEnumerable<TaskDataModel> GetTasksByProjectId(Guid projectId)
        {
            return _tasks
                .Where(t => t.ProjectId == projectId)
                .Select(t => new TaskDataModel(t.Id, t.ProjectId, t.Name, t.Description, t.Priority, t.DueDate, t.IsCompleted));
        }

        public int GetTasksCountByProjectId(Guid projectId)
        {
            return _tasks.Count(t => t.ProjectId == projectId);
        }
    }
}