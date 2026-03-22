using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Common.Enums;
using Xunit;

using System;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Services.DTOModels.Tasks;
using Xunit;

namespace KMA.TaskManager.Tests.DTOModels;

public class TaskDTOTest
{
    [Fact]
    public void TaskDTOs_Initialization_StoresCorrectValues()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var dueDate = DateTimeOffset.Now.AddDays(-1);

        // Act - Створюємо обидві DTO
        var detailsDto = new TaskDetailsDto(
            taskId, projectId, "Деталі завдання", "Опис",
            TaskPriority.High, dueDate, true, true);

        var listDto = new TaskListDTO(
            taskId, "Список завдання", TaskPriority.Medium, false, false);

        // Assert - Перевіряємо TaskDetailsDto
        Assert.Equal(taskId, detailsDto.Id);
        Assert.Equal(projectId, detailsDto.ProjectId);
        Assert.True(detailsDto.IsCompleted);
        Assert.True(detailsDto.IsOverdue);
        Assert.Equal(TaskPriority.High, detailsDto.Priority);

        // Assert - Перевіряємо TaskListDTO
        Assert.Equal(taskId, listDto.Id);
        Assert.Equal("Список завдання", listDto.Name);
        Assert.False(listDto.IsCompleted);
        Assert.Equal(TaskPriority.Medium, listDto.Priority);
    }
}