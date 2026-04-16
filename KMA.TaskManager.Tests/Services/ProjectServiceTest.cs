using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.DTOModels.Tasks;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Repositories.Interfaces;
using KMA.TaskManager.Services;
using KMA.TaskManager.Services.Interfaces;
using Moq;

namespace KMA.TaskManager.Tests.Services;

public class ProjectServiceTest
{
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<ITaskService> _taskServiceMock;
    private readonly Mock<IProjectMapper> _projectMapperMock;
    private readonly ProjectService _service;

    public ProjectServiceTest()
    {
        _projectRepoMock = new Mock<IProjectRepository>();
        _taskServiceMock = new Mock<ITaskService>();
        _projectMapperMock = new Mock<IProjectMapper>();

        _service = new ProjectService(_projectRepoMock.Object, _taskServiceMock.Object, _projectMapperMock.Object);
    }

    [Fact]
    public async Task GetProjectDetailsAsync_ExistingId_ReturnsProjectDetailsDTO()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var dataModel = new ProjectDataModel("Project", "Desc", ProjectType.Work) { Id = projectId };

        var tasks = new List<TaskListDTO>
        {
            new TaskListDTO(Guid.NewGuid(), "T1", TaskPriority.Low, false, false, DateTime.Now)
        };

        var expectedDto = new ProjectDetailsDTO(
            projectId,
            "Project",
            "Desc",
            ProjectType.Work,
            tasks,
            1,
            0,
            0.0,
            "0 з 1 завдань завершено"
        );

        _projectRepoMock.Setup(r => r.GetProjectByIdAsync(projectId)).ReturnsAsync(dataModel);
        _taskServiceMock.Setup(s => s.GetTasksByProjectIdAsync(projectId)).ReturnsAsync(tasks);

        _projectMapperMock.Setup(m => m.MapToDetailsDTO(dataModel, It.IsAny<List<TaskListDTO>>())).Returns(expectedDto);

        // Act
        var result = await _service.GetProjectDetailsAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        Assert.Equal("Project", result.Name);

        _projectRepoMock.Verify(r => r.GetProjectByIdAsync(projectId), Times.Once);
        _taskServiceMock.Verify(s => s.GetTasksByProjectIdAsync(projectId), Times.Once);
        _projectMapperMock.Verify(m => m.MapToDetailsDTO(dataModel, It.IsAny<List<TaskListDTO>>()), Times.Once);
    }

    [Fact]
    public async Task GetProjectDetailsAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepoMock.Setup(r => r.GetProjectByIdAsync(projectId)).ReturnsAsync((ProjectDataModel)null);

        // Act
        var result = await _service.GetProjectDetailsAsync(projectId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ReturnsMappedProjectsList()
    {
        // Arrange
        var project1 = new ProjectDataModel("P1", "D1", ProjectType.Work) { Id = Guid.NewGuid() };
        var project2 = new ProjectDataModel("P2", "D2", ProjectType.Personal) { Id = Guid.NewGuid() };
        var projectsData = new List<ProjectDataModel> { project1, project2 };

        var p1Tasks = new List<TaskListDTO>
        {
            new TaskListDTO(Guid.NewGuid(), "T1", TaskPriority.Low, true, false, DateTime.Now),
            new TaskListDTO(Guid.NewGuid(), "T2", TaskPriority.Low, false, false, DateTime.Now)
        };
        var p2Tasks = new List<TaskListDTO>();

        var expectedDto1 = new ProjectListDTO(project1.Id, "P1", 2, 1, 50.0);
        var expectedDto2 = new ProjectListDTO(project2.Id, "P2", 0, 0, 0.0);

        _projectRepoMock.Setup(r => r.GetAllProjectsAsync()).ReturnsAsync(projectsData);
        _taskServiceMock.Setup(s => s.GetTasksByProjectIdAsync(project1.Id)).ReturnsAsync(p1Tasks);
        _taskServiceMock.Setup(s => s.GetTasksByProjectIdAsync(project2.Id)).ReturnsAsync(p2Tasks);

        _projectMapperMock.Setup(m => m.MapToListDTO(project1, 2, 1)).Returns(expectedDto1);
        _projectMapperMock.Setup(m => m.MapToListDTO(project2, 0, 0)).Returns(expectedDto2);

        // Act
        var result = (await _service.GetAllProjectsAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Contains(expectedDto1, result);
        Assert.Contains(expectedDto2, result);

        _projectRepoMock.Verify(r => r.GetAllProjectsAsync(), Times.Once);
        _taskServiceMock.Verify(s => s.GetTasksByProjectIdAsync(It.IsAny<Guid>()), Times.Exactly(2));
        _projectMapperMock.Verify(m => m.MapToListDTO(It.IsAny<ProjectDataModel>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
    }

    [Fact]
    public async Task CreateProjectAsync_ValidModel_CreatesAndReturnsDetails()
    {
        // Arrange
        var createModel = new ProjectCreateModel("New", "Desc", ProjectType.Work);
        var savedDataModel = new ProjectDataModel(createModel.Name, createModel.Description, createModel.ProjectType) { Id = Guid.NewGuid() };

        var emptyTasks = new List<TaskListDTO>();
        var expectedDto = new ProjectDetailsDTO(
            savedDataModel.Id,
            "New",
            "Desc",
            ProjectType.Work,
            emptyTasks,
            0,
            0,
            0.0,
            "0 з 0 завдань завершено"
        );

        _projectMapperMock.Setup(m => m.MapToData(createModel)).Returns(savedDataModel);
        _projectRepoMock.Setup(r => r.SaveProjectAsync(savedDataModel)).ReturnsAsync(savedDataModel);

        _projectRepoMock.Setup(r => r.GetProjectByIdAsync(savedDataModel.Id)).ReturnsAsync(savedDataModel);
        _taskServiceMock.Setup(s => s.GetTasksByProjectIdAsync(savedDataModel.Id)).ReturnsAsync(emptyTasks);

        _projectMapperMock.Setup(m => m.MapToDetailsDTO(savedDataModel, emptyTasks)).Returns(expectedDto);

        // Act
        var result = await _service.CreateProjectAsync(createModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New", result.Name);

        _projectRepoMock.Verify(r => r.SaveProjectAsync(savedDataModel), Times.Once);
        _projectMapperMock.Verify(m => m.MapToData(createModel), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_ExistingProject_UpdatesAndReturnsDetails()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var editModel = new ProjectEditModel(projectId, "Updated", "Desc", ProjectType.Work);

        var existingData = new ProjectDataModel("Old", "Old", ProjectType.Personal) { Id = projectId };

        var emptyTasks = new List<TaskListDTO>();
        var expectedDto = new ProjectDetailsDTO(
            projectId,
            "Updated",
            "Desc",
            ProjectType.Work,
            emptyTasks,
            0,
            0,
            0.0,
            "0 з 0 завдань завершено"
        );

        _projectRepoMock.Setup(r => r.GetProjectByIdAsync(projectId)).ReturnsAsync(existingData);
        _projectRepoMock.Setup(r => r.SaveProjectAsync(existingData)).ReturnsAsync(existingData);
        _taskServiceMock.Setup(s => s.GetTasksByProjectIdAsync(projectId)).ReturnsAsync(emptyTasks);

        _projectMapperMock.Setup(m => m.MapToDetailsDTO(existingData, emptyTasks)).Returns(expectedDto);

        // Act
        var result = await _service.UpdateProjectAsync(editModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
        _projectRepoMock.Verify(r => r.SaveProjectAsync(existingData), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_NonExistingProject_ThrowsArgumentException()
    {
        // Arrange
        var editModel = new ProjectEditModel(Guid.NewGuid(), "Updated", "Desc", ProjectType.Work);
        _projectRepoMock.Setup(r => r.GetProjectByIdAsync(editModel.Id)).ReturnsAsync((ProjectDataModel)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateProjectAsync(editModel));
    }

    [Fact]
    public async Task DeleteProjectAsync_ValidId_DeletesTasksAndProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _taskServiceMock.Setup(s => s.DeleteTasksByProjectIdAsync(projectId)).ReturnsAsync(true);
        _projectRepoMock.Setup(r => r.DeleteProjectAsync(projectId)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteProjectAsync(projectId);

        // Assert
        Assert.True(result);
        _taskServiceMock.Verify(s => s.DeleteTasksByProjectIdAsync(projectId), Times.Once);
        _projectRepoMock.Verify(r => r.DeleteProjectAsync(projectId), Times.Once);
    }
}