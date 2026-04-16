using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DTOModels.Projects;
using KMA.TaskManager.DTOModels.Tasks;
using System;
using System.Collections.Generic;
using Xunit;

namespace KMA.TaskManager.Tests.DTOModels;

public class ProjectDTOTest
{
    [Fact]
    public void ProjectDetailsDTO_ProgressFraction_HalfTasksCompleted_ReturnsHalf()
    {
        // Arrange
        var tasks = new List<TaskListDTO>
        {
            new TaskListDTO(Guid.NewGuid(), "Task 1", TaskPriority.Medium, true, false, DateTime.Now),
            new TaskListDTO(Guid.NewGuid(), "Task 2", TaskPriority.Medium, false, false, DateTime.Now)
        };
        var project = new ProjectDetailsDTO(Guid.NewGuid(), "Test", "Desc", ProjectType.Work, tasks, 2, 1, 0.5, "1 з 2 завдань завершено");

        // Act
        var result = project.ProgressFraction;

        // Assert
        Assert.Equal(0.5, result);
    }

    [Fact]
    public void ProjectDetailsDTO_ProgressFraction_NoTasks_ReturnsZero()
    {
        // Arrange
        var emptyTasks = new List<TaskListDTO>();
        var project = new ProjectDetailsDTO(Guid.NewGuid(), "Empty", "Desc", ProjectType.Personal, emptyTasks, 0, 0, 0.0, "0 з 0 завдань завершено");

        // Act
        var result = project.ProgressFraction;

        // Assert
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void ProjectListDTO_Progress_AllTasksCompleted_ReturnsHundredPercent()
    {
        // Arrange
        var project = new ProjectListDTO(Guid.NewGuid(), "Full", 4, 4, 100.0);

        // Act
        var result = project.Progress;

        // Assert
        Assert.Equal(100.0, result);
    }
}