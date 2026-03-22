using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Mappers;
using System;
using Xunit;

namespace KMA.TaskManager.Tests.Mappers;

public class TaskMapperTest
{
    private readonly TaskMapper _mapper;

    public TaskMapperTest()
    {
        _mapper = new TaskMapper();
    }

    // 1. Тести на обробку Null (Захист від NullReferenceException)
    [Fact]
    public void MapToData_NullInput_ReturnsNull()
    {
        Assert.Null(_mapper.MapToData(null));
    }

    [Fact]
    public void MapToListDTO_NullInput_ReturnsNull()
    {
        Assert.Null(_mapper.MapToListDTO(null));
    }

    [Fact]
    public void MapToDetailsDTO_NullInput_ReturnsNull()
    {
        Assert.Null(_mapper.MapToDetailsDTO(null));
    }

    // 2. Тести для MapToData (Створення)
    [Fact]
    public void MapToData_ValidModel_SetsIsCompletedToFalse()
    {
        // Arrange
        var createModel = new TaskCreateModel(
            Guid.NewGuid(),
            "New Task",
            "Desc",
            TaskPriority.High,
            DateTimeOffset.Now.AddDays(2));

        // Act
        var result = _mapper.MapToData(createModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createModel.ProjectId, result.ProjectId);
        Assert.Equal(createModel.Name, result.Name);
        Assert.False(result.IsCompleted);
    }

    // 3. Тести логіки IsOverdue (можна перевіряти через Details або List DTO)
    [Fact]
    public void MapToDetailsDTO_PastDueDateAndNotCompleted_IsOverdueIsTrue()
    {
        // Arrange
        var pastDate = DateTimeOffset.Now.AddDays(-1);
        var data = new TaskDataModel(
            Guid.NewGuid(), "Task", "Desc",
            TaskPriority.Medium, pastDate, false); // false = не завершено

        // Act
        var result = _mapper.MapToDetailsDTO(data);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsOverdue);
    }

    [Fact]
    public void MapToDetailsDTO_PastDueDateButCompleted_IsOverdueIsFalse()
    {
        // Arrange
        var pastDate = DateTimeOffset.Now.AddDays(-1);
        var data = new TaskDataModel(
            Guid.NewGuid(), "Task", "Desc",
            TaskPriority.Medium, pastDate, true); // true = завершено

        // Act
        var result = _mapper.MapToDetailsDTO(data);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsOverdue);
    }

    [Fact]
    public void MapToDetailsDTO_FutureDueDate_IsOverdueIsFalse()
    {
        // Arrange
        var futureDate = DateTimeOffset.Now.AddDays(1);
        var data = new TaskDataModel(
            Guid.NewGuid(), "Task", "Desc",
            TaskPriority.Medium, futureDate, false);

        // Act
        var result = _mapper.MapToDetailsDTO(data);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsOverdue);
    }

    // 4. Тести на правильність мапінгу полів
    [Fact]
    public void MapToListDTO_ValidData_MapsCorrectly()
    {
        // Arrange
        var data = new TaskDataModel(
            Guid.NewGuid(), "List Task", "Desc",
            TaskPriority.Critical, DateTimeOffset.Now.AddDays(2), false);

        // Act
        var result = _mapper.MapToListDTO(data);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Id, result.Id);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Priority, result.Priority);
        Assert.Equal(data.IsCompleted, result.IsCompleted);
        Assert.False(result.IsOverdue);
    }

    [Fact]
    public void MapToDetailsDTO_ValidData_MapsAllFieldsCorrectly()
    {
        // Arrange
        var data = new TaskDataModel(
            Guid.NewGuid(), "Details Task", "Description",
            TaskPriority.Low, DateTimeOffset.Now.AddDays(5), false);

        // Act
        var result = _mapper.MapToDetailsDTO(data);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Id, result.Id);
        Assert.Equal(data.ProjectId, result.ProjectId);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Description, result.Description);
        Assert.Equal(data.Priority, result.Priority);
        Assert.Equal(data.DueDate, result.DueDate);
        Assert.Equal(data.IsCompleted, result.IsCompleted);
    }
}