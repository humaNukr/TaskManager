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
    // Обов'язково вказуємо реалізацію інтерфейсу IStorageContext
    public class SQLLiteStorageContext : IStorageContext
    {
        private const string DatabaseFileName = "task_manager.db3";

        private static readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "DB Storage", DatabaseFileName);

        private SQLiteAsyncConnection _databaseConnection;

        // SemaphoreSlim гарантує, що лише один потік може ініціалізувати або писати в БД одночасно
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

        private async Task CreateMockStorage()
        {
            var dir = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _databaseConnection = new SQLiteAsyncConnection(DatabasePath);

            await _databaseConnection.CreateTableAsync<ProjectDataModel>();
            await _databaseConnection.CreateTableAsync<TaskDataModel>();

            var inMemoryStorage = new InMemoryStorageContext();

            foreach (var project in inMemoryStorage.GetProjects())
            {
                await _databaseConnection.InsertAsync(project);

                var tasks = inMemoryStorage.GetTasksByProjectId(project.Id);
                await _databaseConnection.InsertAllAsync(tasks);
            }
        }

        #endregion

        #region Tasks

        public async Task<TaskDataModel?> GetTaskByIdAsync(Guid id)
        {
            await Init();
            return await _databaseConnection.Table<TaskDataModel>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskDataModel>> GetTasksByProjectIdAsync(Guid projectId)
        {
            await Init();
            return await _databaseConnection.Table<TaskDataModel>().Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<int> GetTasksCountByProjectIdAsync(Guid projectId)
        {
            await Init();
            return await _databaseConnection.Table<TaskDataModel>().CountAsync(t => t.ProjectId == projectId);
        }

        public async Task SaveTaskAsync(TaskDataModel task)
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
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            await Init();
            await _databaseConnection.DeleteAsync<TaskDataModel>(taskId);
        }

        #endregion

        #region Projects

        public async Task<ProjectDataModel?> GetProjectByIdAsync(Guid id)
        {
            await Init();
            return await _databaseConnection.Table<ProjectDataModel>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async IAsyncEnumerable<ProjectDataModel> GetProjectsAsync()
        {
            await Init();
            var projects = await _databaseConnection.Table<ProjectDataModel>().ToListAsync();
            foreach (var project in projects)
            {
                yield return project;
            }
        }

        public async Task SaveProjectAsync(ProjectDataModel project)
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
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            await Init();

            // Каскадне видалення: спочатку видаляємо всі таски цього проєкту, потім сам проєкт
            var tasks = await GetTasksByProjectIdAsync(projectId);
            foreach (var task in tasks)
            {
                await DeleteTaskAsync(task.Id);
            }

            await _databaseConnection.DeleteAsync<ProjectDataModel>(projectId);
        }

        #endregion
    }
}