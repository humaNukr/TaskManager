using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.DTOModels.Tasks;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;

namespace KMA.TaskManager.Tests.Services;

public class TaskServiceTest
{
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly Mock<ITaskMapper> _taskMapperMock;
    private readonly TaskService _service;

    public TaskServiceTest()
    {
        _taskRepoMock = new Mock<ITaskRepository>();
        _taskMapperMock = new Mock<ITaskMapper>();
        _service = new TaskService(_taskRepoMock.Object, _taskMapperMock.Object);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ExistingId_ReturnsMappedTaskDetailsDTO()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var dueDateOffset = DateTimeOffset.Now;
        var dueDate = dueDateOffset.DateTime;

        var dataModel = new TaskDataModel(taskId, projectId, "Test Task", "Desc", TaskPriority.High, dueDateOffset, false);

        var expectedDto = new TaskDetailsDto(
            taskId, projectId, "Test Task", "Desc", TaskPriority.High, dueDate, false, false);

        _taskRepoMock.Setup(r => r.GetTaskByIdAsync(taskId)).ReturnsAsync(dataModel);
        _taskMapperMock.Setup(m => m.MapToDetailsDTO(dataModel)).Returns(expectedDto);

        // Act
        var result = await _service.GetTaskByIdAsync(taskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        Assert.Equal("Test Task", result.Name);

        _taskRepoMock.Verify(r => r.GetTaskByIdAsync(taskId), Times.Once);
        _taskMapperMock.Verify(m => m.MapToDetailsDTO(dataModel), Times.Once);
    }

    [Fact]
    public async Task GetTaskByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _taskRepoMock.Setup(r => r.GetTaskByIdAsync(taskId)).ReturnsAsync((TaskDataModel?)null);

        // Act
        var result = await _service.GetTaskByIdAsync(taskId);

        // Assert
        Assert.Null(result);
        _taskRepoMock.Verify(r => r.GetTaskByIdAsync(taskId), Times.Once);
        _taskMapperMock.Verify(m => m.MapToDetailsDTO(It.IsAny<TaskDataModel>()), Times.Never);
    }

    [Fact]
    public async Task GetTasksByProjectIdAsync_ReturnsMappedTasksList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var dueDateOffset = DateTimeOffset.Now;
        var dueDate = dueDateOffset.DateTime;

        var task1 = new TaskDataModel(Guid.NewGuid(), projectId, "Task 1", "D1", TaskPriority.High, dueDateOffset, false);
        var task2 = new TaskDataModel(Guid.NewGuid(), projectId, "Task 2", "D2", TaskPriority.Low, dueDateOffset, true);
        var tasksData = new List<TaskDataModel> { task1, task2 };

        var dto1 = new TaskListDTO(task1.Id, "Task 1", TaskPriority.High, false, false, dueDate);
        var dto2 = new TaskListDTO(task2.Id, "Task 2", TaskPriority.Low, true, false, dueDate);

        _taskRepoMock.Setup(r => r.GetTasksByProjectIdAsync(projectId)).ReturnsAsync(tasksData);

        _taskMapperMock.Setup(m => m.MapToListDTO(task1)).Returns(dto1);
        _taskMapperMock.Setup(m => m.MapToListDTO(task2)).Returns(dto2);

        // Act
        var result = (await _service.GetTasksByProjectIdAsync(projectId)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(dto1, result);
        Assert.Contains(dto2, result);

        _taskRepoMock.Verify(r => r.GetTasksByProjectIdAsync(projectId), Times.Once);
        _taskMapperMock.Verify(m => m.MapToListDTO(It.IsAny<TaskDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetTasksByProjectIdAsync_NoTasksForProject_ReturnsEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.GetTasksByProjectIdAsync(projectId)).ReturnsAsync(new List<TaskDataModel>());

        // Act
        var result = (await _service.GetTasksByProjectIdAsync(projectId)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _taskRepoMock.Verify(r => r.GetTasksByProjectIdAsync(projectId), Times.Once);
        _taskMapperMock.Verify(m => m.MapToListDTO(It.IsAny<TaskDataModel>()), Times.Never);
    }

    [Fact]
    public async Task CreateTaskAsync_ValidModel_CreatesAndReturnsDetails()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var createModel = new TaskCreateModel(projectId, "New Task", "Desc", TaskPriority.Medium, DateTimeOffset.Now);

        var dataModel = new TaskDataModel(Guid.NewGuid(), projectId, "New Task", "Desc", TaskPriority.Medium, DateTimeOffset.Now, false);
        var expectedDto = new TaskDetailsDto(dataModel.Id, projectId, "New Task", "Desc", TaskPriority.Medium, DateTime.Now, false, false);

        _taskMapperMock.Setup(m => m.MapToData(createModel)).Returns(dataModel);
        _taskRepoMock.Setup(r => r.SaveTaskAsync(dataModel)).ReturnsAsync(dataModel);
        _taskMapperMock.Setup(m => m.MapToDetailsDTO(dataModel)).Returns(expectedDto);

        // Act
        var result = await _service.CreateTaskAsync(createModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Task", result.Name);

        _taskMapperMock.Verify(m => m.MapToData(createModel), Times.Once);
        _taskRepoMock.Verify(r => r.SaveTaskAsync(dataModel), Times.Once);
        _taskMapperMock.Verify(m => m.MapToDetailsDTO(dataModel), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ExistingTask_UpdatesAndReturnsDetails()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var editModel = new TaskEditModel(taskId, "Updated Task", "Desc", TaskPriority.High, DateTimeOffset.Now, true);

        var existingData = new TaskDataModel(taskId, projectId, "Old Task", "Old Desc", TaskPriority.Low, DateTimeOffset.Now, false);
        
        var updatedDto = new TaskDetailsDto(
            taskId,
            projectId,
            "Updated Task",
            "Desc",
            TaskPriority.High,
            DateTimeOffset.Now,
            true,
            false
        );

        _taskRepoMock.Setup(r => r.GetTaskByIdAsync(taskId)).ReturnsAsync(existingData);
        _taskRepoMock.Setup(r => r.SaveTaskAsync(existingData)).ReturnsAsync(existingData);
        _taskMapperMock.Setup(m => m.MapToDetailsDTO(existingData)).Returns(updatedDto);

        // Act
        var result = await _service.UpdateTaskAsync(editModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Task", result.Name);
        Assert.True(result.IsCompleted);

        _taskRepoMock.Verify(r => r.GetTaskByIdAsync(taskId), Times.Once);
        _taskMapperMock.Verify(m => m.MapUpdateToData(editModel, existingData), Times.Once);
        _taskRepoMock.Verify(r => r.SaveTaskAsync(existingData), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_NonExistingTask_ReturnsNull()
    {
        // Arrange
        var editModel = new TaskEditModel(Guid.NewGuid(), "Updated Task", "Desc", TaskPriority.High, DateTimeOffset.Now, true);
        _taskRepoMock.Setup(r => r.GetTaskByIdAsync(editModel.Id)).ReturnsAsync((TaskDataModel?)null);

        // Act
        var result = await _service.UpdateTaskAsync(editModel);

        // Assert
        Assert.Null(result);
        _taskRepoMock.Verify(r => r.SaveTaskAsync(It.IsAny<TaskDataModel>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTaskAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.DeleteTaskAsync(taskId)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteTaskAsync(taskId);

        // Assert
        Assert.True(result);
        _taskRepoMock.Verify(r => r.DeleteTaskAsync(taskId), Times.Once);
    }

    [Fact]
    public async Task DeleteTasksByProjectIdAsync_ValidProjectId_ReturnsTrue()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.DeleteTasksByProjectIdAsync(projectId)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteTasksByProjectIdAsync(projectId);

        // Assert
        Assert.True(result);
        _taskRepoMock.Verify(r => r.DeleteTasksByProjectIdAsync(projectId), Times.Once);
    }
}