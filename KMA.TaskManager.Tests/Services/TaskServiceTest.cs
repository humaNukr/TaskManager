using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.DTOModels.Tasks;
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
    public void GetTaskById_ExistingId_ReturnsMappedTaskDetailsDTO()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var dataModel = new TaskDataModel(projectId, "Test Task", "Desc", TaskPriority.High, DateTimeOffset.Now, false);
        var taskId = dataModel.Id;

        var expectedDto = new TaskDetailsDto(
            taskId, projectId, "Test Task", "Desc", TaskPriority.High, DateTimeOffset.Now, false, false);
        _taskRepoMock.Setup(r => r.GetTaskById(taskId)).Returns(dataModel);
        _taskMapperMock.Setup(m => m.MapToDetailsDTO(dataModel)).Returns(expectedDto);

        // Act
        var result = _service.GetTaskById(taskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        Assert.Equal("Test Task", result.Name);

        // Перевіряємо, що репозиторій і мапер були викликані правильно
        _taskRepoMock.Verify(r => r.GetTaskById(taskId), Times.Once);
        _taskMapperMock.Setup(m => m.MapToDetailsDTO(dataModel));
    }

    [Fact]
    public void GetTaskById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        _taskRepoMock.Setup(r => r.GetTaskById(taskId)).Returns((TaskDataModel?)null);
        _taskMapperMock.Setup(m => m.MapToDetailsDTO(It.IsAny<TaskDataModel>())).Returns((TaskDetailsDto?)null);

        // Act
        var result = _service.GetTaskById(taskId);

        // Assert
        Assert.Null(result);
        _taskRepoMock.Verify(r => r.GetTaskById(taskId), Times.Once);
    }

    [Fact]
    public void GetTasksByProjectId_ReturnsMappedTasksList()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        var task1 = new TaskDataModel(projectId, "Task 1", "D1", TaskPriority.High, DateTimeOffset.Now, false);
        var task2 = new TaskDataModel(projectId, "Task 2", "D2", TaskPriority.Low, DateTimeOffset.Now, true);
        var tasksData = new List<TaskDataModel> { task1, task2 };

        var dto1 = new TaskListDTO(task1.Id, "Task 1", TaskPriority.High, false, false);
        var dto2 = new TaskListDTO(task2.Id, "Task 2", TaskPriority.Low, true, false);

        _taskRepoMock.Setup(r => r.GetTasksByProjectId(projectId)).Returns(tasksData);

        // Налаштовуємо мапер для кожного об'єкта
        _taskMapperMock.Setup(m => m.MapToListDTO(task1)).Returns(dto1);
        _taskMapperMock.Setup(m => m.MapToListDTO(task2)).Returns(dto2);

        // Act
        // Оскільки метод повертає IEnumerable через yield return, викликаємо ToList(), щоб виконати ітерацію
        var result = _service.GetTasksByProjectId(projectId).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(dto1, result);
        Assert.Contains(dto2, result);

        // Перевіряємо, що репозиторій викликався один раз для проєкту
        _taskRepoMock.Verify(r => r.GetTasksByProjectId(projectId), Times.Once);

        // Перевіряємо, що мапер викликався рівно стільки разів, скільки елементів у списку
        _taskMapperMock.Verify(m => m.MapToListDTO(It.IsAny<TaskDataModel>()), Times.Exactly(2));
    }

    [Fact]
    public void GetTasksByProjectId_NoTasksForProject_ReturnsEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Налаштовуємо репозиторій повертати порожній список
        _taskRepoMock.Setup(r => r.GetTasksByProjectId(projectId)).Returns(new List<TaskDataModel>());

        // Act
        var result = _service.GetTasksByProjectId(projectId).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // Список має бути порожнім

        _taskRepoMock.Verify(r => r.GetTasksByProjectId(projectId), Times.Once);
        // Мапер не повинен викликатися жодного разу, бо список порожній
        _taskMapperMock.Verify(m => m.MapToListDTO(It.IsAny<TaskDataModel>()), Times.Never);
    }
}