using System;
using System.Collections.Generic;
using Moq;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Repositories.Interfaces;
using Xunit;

namespace KMA.TaskManager.Tests.Services;

public class ProjectServiceTest
{
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly Mock<IProjectMapper> _projectMapperMock;
    private readonly Mock<ITaskMapper> _taskMapperMock;
    private readonly ProjectService _service;

    public ProjectServiceTest()
    {
        _projectRepoMock = new Mock<IProjectRepository>();
        _taskRepoMock = new Mock<ITaskRepository>();
        _projectMapperMock = new Mock<IProjectMapper>();
        _taskMapperMock = new Mock<ITaskMapper>();

        _service = new ProjectService(
            _projectRepoMock.Object,
            _taskRepoMock.Object,
            _projectMapperMock.Object,
            _taskMapperMock.Object);
    }

    [Fact]
    public void GetProjectById_ExistingId_ReturnsMappedProjectDetailsDTO()
    {
        // Arrange
        var dataModel = new ProjectDataModel("Project", "Desc", ProjectType.Work);
        var projectId = dataModel.Id;

        var tasksData = new List<TaskDataModel>
        {
            new TaskDataModel(projectId, "T1", "D1", TaskPriority.Low, DateTimeOffset.Now, true)
        };

        var taskDto = new TaskListDTO(tasksData[0].Id, "T1", TaskPriority.Low, false, true);
        var expectedDto = new ProjectDetailsDTO(projectId, "Project", "Desc", ProjectType.Work, new List<TaskListDTO> { taskDto });

        _projectRepoMock.Setup(r => r.GetProjectById(projectId)).Returns(dataModel);
        _taskRepoMock.Setup(r => r.GetTasksByProjectId(projectId)).Returns(tasksData);
        _taskMapperMock.Setup(m => m.MapToListDTO(tasksData[0])).Returns(taskDto);
        _projectMapperMock.Setup(m => m.MapToDetailsDTO(dataModel, It.IsAny<IEnumerable<TaskListDTO>>())).Returns(expectedDto);

        // Act
        var result = _service.GetProjectById(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        _projectRepoMock.Verify(r => r.GetProjectById(projectId), Times.Once);
        _taskRepoMock.Verify(r => r.GetTasksByProjectId(projectId), Times.Once);
    }

    [Fact]
    public void GetProjectById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepoMock.Setup(r => r.GetProjectById(projectId)).Returns((ProjectDataModel)null);

        // Act
        var result = _service.GetProjectById(projectId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllProjects_ReturnsMappedProjectsList()
    {
        // Arrange
        var project1 = new ProjectDataModel("P1", "D1", ProjectType.Work);
        var project2 = new ProjectDataModel("P2", "D2", ProjectType.Personal);
        var projectsData = new List<ProjectDataModel> { project1, project2 };

        var p1Tasks = new List<TaskDataModel>
        {
            new TaskDataModel(project1.Id, "T1", "D1", TaskPriority.Low, DateTimeOffset.Now, true)
        };
        var p2Tasks = new List<TaskDataModel>();

        var dto1 = new ProjectListDTO(project1.Id, "P1", 1, 1);
        var dto2 = new ProjectListDTO(project2.Id, "P2", 0, 0);

        _projectRepoMock.Setup(r => r.GetProjects()).Returns(projectsData);
        _taskRepoMock.Setup(r => r.GetTasksByProjectId(project1.Id)).Returns(p1Tasks);
        _taskRepoMock.Setup(r => r.GetTasksByProjectId(project2.Id)).Returns(p2Tasks);

        _projectMapperMock.Setup(m => m.MapToListDTO(project1, 1, 1)).Returns(dto1);
        _projectMapperMock.Setup(m => m.MapToListDTO(project2, 0, 0)).Returns(dto2);

        // Act
        var result = _service.GetAllProjects().ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(dto1, result);
        Assert.Contains(dto2, result);

        _projectRepoMock.Verify(r => r.GetProjects(), Times.Once);
        _taskRepoMock.Verify(r => r.GetTasksByProjectId(It.IsAny<Guid>()), Times.Exactly(2));
    }
}