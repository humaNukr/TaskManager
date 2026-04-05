using KMA.TaskManager.Common.Enums; // Переконайся, що тут правильний namespace для твоїх Enum
using KMA.TaskManager.DataModels;
using Microsoft.Maui.Storage;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KMA.TaskManager.Storage
{
    public class SQLLiteStorageContext : IStorageContext
    {
        private const string DatabaseFileName = "task_manager.db3";

        private static readonly string DatabasePath =
            Path.Combine(FileSystem.AppDataDirectory, "DB Storage", DatabaseFileName);

        private SQLiteAsyncConnection _databaseConnection;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        #region Initialization

        private async Task Init()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (_databaseConnection is not null)
                    return;

                bool isFirstLaunch = !File.Exists(DatabasePath);

                if (isFirstLaunch)
                    await CreateMockStorage();
                else
                    _databaseConnection = new SQLiteAsyncConnection(DatabasePath);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #endregion

        #region MockStorage

        private async Task CreateMockStorage()
        {
            var dir = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _databaseConnection = new SQLiteAsyncConnection(DatabasePath);

            await _databaseConnection.CreateTableAsync<ProjectDataModel>();
            await _databaseConnection.CreateTableAsync<TaskDataModel>();

            // 1. Створюємо проєкти із вказанням ProjectType
            var bakeryWebsite = new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Name = "Розробка вебсайту пекарні",
                Description = "Створення сайту-візитки з каталогом продукції для місцевої кондитерської \"Зефір\"",
                ProjectType = ProjectType.Work
            };

            var csharpCourse = new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Name = "Курс програмування на C#",
                Description = "Виконання лабораторних та практичних робіт у рамках першого семестру",
                ProjectType = ProjectType.Educational
            };

            var homeRenovation = new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Name = "Ремонт у вітальні",
                Description = "Планування бюджету, вибір матеріалів та пошук майстрів для оновлення інтер'єру",
                ProjectType = ProjectType.Personal
            };

            var aiMarketResearch = new ProjectDataModel
            {
                Id = Guid.NewGuid(),
                Name = "Дослідження ринку ШІ",
                Description = "Аналіз сучасних трендів у сфері штучного інтелекту для написання наукової статті",
                ProjectType = ProjectType.Research
            };

            var projects = new List<ProjectDataModel> { bakeryWebsite, csharpCourse, homeRenovation, aiMarketResearch };

            await _databaseConnection.InsertAllAsync(projects);

            // 2. Створюємо завдання
            var tasks = new List<TaskDataModel>
            {
                // Завдання для пекарні
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Аналіз вимог",
                    Description = "Зустріч з власником для ТЗ", Priority = TaskPriority.High,
                    DueDate = DateTimeOffset.Now.AddDays(1), IsCompleted = true
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Макет дизайну",
                    Description = "Розробка стилю в Figma", Priority = TaskPriority.Medium,
                    DueDate = DateTimeOffset.Now.AddDays(5), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Верстка головної",
                    Description = "HTML/CSS адаптивна верстка", Priority = TaskPriority.High,
                    DueDate = DateTimeOffset.Now.AddDays(7), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Каталог товарів",
                    Description = "Розробка сторінки з випічкою", Priority = TaskPriority.Medium,
                    DueDate = DateTimeOffset.Now.AddDays(10), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Форма замовлення",
                    Description = "Логіка відправки запитів на email", Priority = TaskPriority.High,
                    DueDate = DateTimeOffset.Now.AddDays(12), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Налаштування SEO",
                    Description = "Оптимізація мета-тегів", Priority = TaskPriority.Low,
                    DueDate = DateTimeOffset.Now.AddDays(15), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Тестування",
                    Description = "Перевірка кросбраузерності", Priority = TaskPriority.High,
                    DueDate = DateTimeOffset.Now.AddDays(16), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Наповнення текстами",
                    Description = "Копірайтинг для розділу про нас", Priority = TaskPriority.Low,
                    DueDate = DateTimeOffset.Now.AddDays(-2), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Фотосесія",
                    Description = "Зйомка десертів для каталогу", Priority = TaskPriority.Medium,
                    DueDate = DateTimeOffset.Now.AddDays(20), IsCompleted = false
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = bakeryWebsite.Id, Name = "Деплой",
                    Description = "Перенесення сайту на хостинг", Priority = TaskPriority.Critical,
                    DueDate = DateTimeOffset.Now.AddDays(21), IsCompleted = false
                },

                // Завдання для курсу C#
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = csharpCourse.Id, Name = "Лабораторна 1",
                    Description = "Реалізація моделей та сервісів", Priority = TaskPriority.High,
                    DueDate = DateTimeOffset.Now.AddDays(3), IsCompleted = true
                },
                new TaskDataModel
                {
                    Id = Guid.NewGuid(), ProjectId = csharpCourse.Id, Name = "Лабораторна 2",
                    Description = "Робота з GUI та подіями", Priority = TaskPriority.High,
                    DueDate = DateTimeOffset.Now.AddDays(14), IsCompleted = false
                }
            };

            await _databaseConnection.InsertAllAsync(tasks);
        }

        #endregion

        #region Tasks

        public async Task<IEnumerable<TaskDataModel>> GetTasksAsync()
        {
            await Init();
            return await _databaseConnection.Table<TaskDataModel>().ToListAsync();
        }

        public async Task<IEnumerable<TaskDataModel>> GetTasksByProjectIdAsync(Guid projectId)
        {
            await Init();
            return await _databaseConnection.Table<TaskDataModel>().Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskDataModel> GetTaskByIdAsync(Guid id)
        {
            await Init();
            return await _databaseConnection.Table<TaskDataModel>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskDataModel> SaveTaskAsync(TaskDataModel task)
        {
            await Init();

            var existingTask = await GetTaskByIdAsync(task.Id);
            if (existingTask != null)
            {
                await _databaseConnection.UpdateAsync(task);
            }
            else
            {
                await _databaseConnection.InsertAsync(task);
            }

            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            await Init();
            var result = await _databaseConnection.DeleteAsync<TaskDataModel>(id);
            return result > 0;
        }

        public async Task<bool> DeleteTasksByProjectIdAsync(Guid projectId)
        {
            await Init();
            var tasks = await GetTasksByProjectIdAsync(projectId);

            int deletedCount = 0;
            foreach (var task in tasks)
            {
                deletedCount += await _databaseConnection.DeleteAsync<TaskDataModel>(task.Id);
            }

            return true;
        }

        #endregion

        #region Projects

        public async Task<IEnumerable<ProjectDataModel>> GetProjectsAsync()
        {
            await Init();
            return await _databaseConnection.Table<ProjectDataModel>().ToListAsync();
        }

        public async Task<ProjectDataModel> GetProjectByIdAsync(Guid id)
        {
            await Init();
            return await _databaseConnection.Table<ProjectDataModel>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProjectDataModel> SaveProjectAsync(ProjectDataModel project)
        {
            await Init();

            var existingProject = await GetProjectByIdAsync(project.Id);
            if (existingProject != null)
            {
                await _databaseConnection.UpdateAsync(project);
            }
            else
            {
                await _databaseConnection.InsertAsync(project);
            }

            return project;
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            await Init();

            await DeleteTasksByProjectIdAsync(id);

            var result = await _databaseConnection.DeleteAsync<ProjectDataModel>(id);
            return result > 0;
        }

        #endregion
    }
}