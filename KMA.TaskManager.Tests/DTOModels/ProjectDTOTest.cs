using System;
using System.Collections.Generic;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.Services.DTOModels.Projects;
using KMA.TaskManager.Services.DTOModels.Tasks;
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
            // Одна таска виконана (true), інша ні (false)
            new TaskListDTO(Guid.NewGuid(), "Task 1", TaskPriority.Medium, true, false, DateTime.Now),
            new TaskListDTO(Guid.NewGuid(), "Task 2", TaskPriority.Medium, false, false, DateTime.Now)
        };
        var project = new ProjectDetailsDTO(Guid.NewGuid(), "Test", "Desc", ProjectType.Work, tasks);

        // Act
        var result = project.ProgressFraction;

        // Assert
        Assert.Equal(0.5, result);
    }

    [Fact]
    public void ProjectDetailsDTO_ProgressFraction_NoTasks_ReturnsZero()
    {
        // Arrange
        var project = new ProjectDetailsDTO(Guid.NewGuid(), "Empty", "Desc", ProjectType.Personal, new List<TaskListDTO>());

        // Act
        var result = project.ProgressFraction;

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ProjectListDTO_Progress_AllTasksCompleted_ReturnsHundredPercent()
    {
        // Arrange
        // У проєкті 4 таски, і всі 4 виконані
        var project = new ProjectListDTO(Guid.NewGuid(), "Full", 4, 4);

        // Act
        var result = project.Progress;

        // Assert
        Assert.Equal(100, result);
    }
}