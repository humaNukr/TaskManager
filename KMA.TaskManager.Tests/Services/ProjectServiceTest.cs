using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Tests.Services;

public class ProjectServiceTest
{
    [Fact]
    public void GetAllProjects_ProjectsExist_ReturnsAllProjectsWithCorrectCounts()
    {
        // Arrange
        var service = new ProjectService();

        // Act
        var result = service.GetAllProjects();

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, project =>
        {
            Assert.True(project.TotalTasksCount >= project.CompletedTasksCount);
        });
    }

    [Fact]
    public void GetProjectById_ExistingId_ReturnsCorrectProjectUIModel()
    {
        // Arrange
        var service = new ProjectService();
        var existingProject = MockStorage.Projects.First();
        var projectId = existingProject.Id;

        // Act
        var result = service.GetProjectById(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        Assert.Equal(existingProject.Name, result.Name);
        Assert.True(result.TotalTasksCount >= result.CompletedTasksCount);
    }

    [Fact]
    public void GetProjectById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var service = new ProjectService();
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = service.GetProjectById(nonExistingId);

        // Assert
        Assert.Null(result);
    }
}