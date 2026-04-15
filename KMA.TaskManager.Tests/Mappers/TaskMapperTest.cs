using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;
using Xunit;

namespace KMA.TaskManager.Tests.Mappers;

public class TaskMapperTest
{
    private readonly TaskMapper _mapper;

    public TaskMapperTest()
    {
        _mapper = new TaskMapper();
    }

    [Fact]
    public void MapToListDTO_ValidDataModel_ReturnsCorrectDTO()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var dueDate = DateTimeOffset.Now.AddDays(2);
        var data = new TaskDataModel(projectId, "Task", "Desc", TaskPriority.High, dueDate, false) { Id = Guid.NewGuid() };

        // Act
        var result = _mapper.MapToListDTO(data);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Id, result.Id);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Priority, result.Priority);
        Assert.Equal(data.IsCompleted, result.IsCompleted);
        Assert.Equal(data.DueDate.DateTime, result.DueDate);
    }

    [Fact]
    public void MapToData_ValidCreateModel_ReturnsCorrectDataModel()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var dueDate = DateTimeOffset.Now.AddDays(1);
        var createModel = new TaskCreateModel(projectId, "New Task", "Task Desc", TaskPriority.Medium, dueDate);

        // Act
        var result = _mapper.MapToData(createModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createModel.ProjectId, result.ProjectId);
        Assert.Equal(createModel.Name, result.Name);
        Assert.Equal(createModel.Description, result.Description);
        Assert.Equal(createModel.Priority, result.Priority);
        Assert.Equal(createModel.DueDate, result.DueDate);
        Assert.False(result.IsCompleted);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}