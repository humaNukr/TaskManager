using Moq;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;
using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Tests.Services;

public class TaskServiceTest
{
    private readonly Mock<IStorageContext> _storageMock;
    private readonly Mock<ITaskMapper> _mapperMock;
    private readonly TaskService _service;

    public TaskServiceTest()
    {
        _storageMock = new Mock<IStorageContext>();
        _mapperMock = new Mock<ITaskMapper>();
        // Впроваджуємо моки в сервіс
        _service = new TaskService(_storageMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetTasksByProjectId_ShouldReturnMappedTasks()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var rawTasks = new List<TaskDataModel>
        {
            new TaskDataModel(projectId, "Data Task", "Desc", TaskPriority.High, DateTimeOffset.Now, false)
        };

        var uiTask = new TaskUIModel(taskId, projectId, "UI Task", "Desc", TaskPriority.High, DateTimeOffset.Now, false);

        _storageMock.Setup(s => s.GetTasksByProjectId(projectId)).Returns(rawTasks);
        _mapperMock.Setup(m => m.MapToUI(It.IsAny<TaskDataModel>())).Returns(uiTask);

        // Act
        var result = _service.GetTasksByProjectId(projectId);

        // Assert
        Assert.Single(result);
        Assert.Equal("UI Task", result[0].Name);
        _storageMock.Verify(s => s.GetTasksByProjectId(projectId), Times.Once); // Перевірка виклику сховища
    }
}