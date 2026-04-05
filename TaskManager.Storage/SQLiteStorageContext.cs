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