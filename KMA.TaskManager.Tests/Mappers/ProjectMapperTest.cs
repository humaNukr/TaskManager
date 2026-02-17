using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;

namespace KMA.TaskManager.Tests.Mappers;

public class ProjectMapperTest
{
    [Fact]
    public void MapToUI_ValidDataModel_ReturnsCorrectUIModel()
    {
        // Arrange
        var data = new ProjectDataModel("Test Project", "Description", ProjectType.Personal);
        int total = 10;
        int completed = 5;

        // Act
        var result = ProjectMapper.MapToUI(data, total, completed);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Id, result.Id);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Description, result.Description);
        Assert.Equal(data.ProjectType, result.ProjectType);
        Assert.Equal(total, result.TotalTasksCount);
        Assert.Equal(completed, result.CompletedTasksCount);
    }

    [Fact]
    public void MapToUI_NullDataModel_ReturnsNull()
    {
        // Arrange
        ProjectDataModel data = null;
        int total = 10;
        int completed = 5;
        // Act
        var result = ProjectMapper.MapToUI(data, total, completed);
        // Assert
        Assert.Null(result);
    }
}