using Moq;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;
using KMA.TaskManager.Common.Enums;
using Xunit;
using KMA.TaskManager.Storage;

namespace KMA.TaskManager.Tests.Services;

public class ProjectServiceTest
{
    private readonly Mock<IStorageContext> _storageMock;
    private readonly Mock<IProjectMapper> _mapperMock;
    private readonly ProjectService _service;

    public ProjectServiceTest()
    {
        _storageMock = new Mock<IStorageContext>();
        _mapperMock = new Mock<IProjectMapper>();
        _service = new ProjectService(_storageMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void GetProjectById_ExistingId_ReturnsMappedProject()
    {
        // Arrange
        var dataModel = new ProjectDataModel("Project", "Desc", ProjectType.Work);
        var projectId = dataModel.Id;

        var tasks = new List<TaskDataModel>
        {
            new TaskDataModel(projectId, "T1", "D1", TaskPriority.Low, DateTimeOffset.Now, true),
            new TaskDataModel(projectId, "T2", "D2", TaskPriority.Low, DateTimeOffset.Now, false)
        };

        var uiModel = new ProjectUIModel(projectId, "Project", "Desc", ProjectType.Work, 2, 1);

        _storageMock.Setup(s => s.GetProjectById(projectId)).Returns(dataModel);
        _storageMock.Setup(s => s.GetTasksByProjectId(projectId)).Returns(tasks);

        _mapperMock.Setup(m => m.MapToUI(dataModel, It.IsAny<int>(), It.IsAny<int>())).Returns(uiModel);

        // Act
        var result = _service.GetProjectById(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        _storageMock.Verify(s => s.GetProjectById(projectId), Times.Once);
    }

    [Fact]
    public void GetProjectById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _storageMock.Setup(s => s.GetProjectById(projectId)).Returns((ProjectDataModel)null);

        // Act
        var result = _service.GetProjectById(projectId);

        // Assert
        Assert.Null(result);
    }
}