using System;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Services.DTOModels.Tasks;
using Xunit;

namespace KMA.TaskManager.Tests.DTOModels;

public class TaskDTOTest
{
    [Fact]
    public void TaskListDTO_Initialization_PropertiesAreSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dueDate = DateTime.Now.AddDays(1);

        // Act
        var dto = new TaskListDTO(id, "Test", TaskPriority.High, true, false, dueDate);

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal("Test", dto.Name);
        Assert.Equal(TaskPriority.High, dto.Priority);
        Assert.True(dto.IsCompleted);
        Assert.False(dto.IsOverdue);
        Assert.Equal(dueDate, dto.DueDate);
    }

    [Fact]
    public void TaskDetailsDto_Initialization_PropertiesAreSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var dueDate = DateTimeOffset.Now.AddDays(-1);

        // Act
        var dto = new TaskDetailsDto(id, projectId, "Name", "Desc", TaskPriority.Low, dueDate, false, true);

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(projectId, dto.ProjectId);
        Assert.Equal("Name", dto.Name);
        Assert.Equal("Desc", dto.Description);
        Assert.Equal(TaskPriority.Low, dto.Priority);
        Assert.False(dto.IsCompleted);
        Assert.True(dto.IsOverdue);
        Assert.Equal(dueDate, dto.DueDate);
    }
}