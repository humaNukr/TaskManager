using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;
using Xunit;

namespace KMA.TaskManager.Tests.Mappers;

public class ProjectMapperTest
{
    private readonly ProjectMapper _mapper;

    public ProjectMapperTest()
    {
        _mapper = new ProjectMapper();
    }

    [Fact]
    public void MapToUI_ValidDataModel_ReturnsCorrectUIModel()
    {
        // Arrange
        var data = new ProjectDataModel("Test Project", "Description", ProjectType.Personal);
        int total = 10;
        int completed = 5;

        // Act
        var result = _mapper.MapToUI(data, total, completed);

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

        // Act
        var result = _mapper.MapToUI(data, 0, 0);

        // Assert
        Assert.Null(result);
    }
}