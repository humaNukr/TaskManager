using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.UIModels;

namespace KMA.TaskManager.Tests.UIModels;

public class ProjectUIModelTest
{
    [Fact]
    public void Progress_HalfTasksCompleted_ReturnsFiftyPercent()
    {
        // Arrange
        var project = new ProjectUIModel(Guid.NewGuid(), "Test", "Desc", ProjectType.Work, 10, 5);

        // Act
        var result = project.Progress;

        // Assert
        Assert.Equal(50, result);
    }

    [Fact]
    public void Progress_NoTasks_ReturnsZero()
    {
        // Arrange
        var project = new ProjectUIModel(Guid.NewGuid(), "Empty", "Desc", ProjectType.Personal, 0, 0);

        // Act
        var result = project.Progress;

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Progress_AllTasksCompleted_ReturnsHundredPercent()
    {
        // Arrange
        var project = new ProjectUIModel(Guid.NewGuid(), "Full", "Desc", ProjectType.Educational, 4, 4);

        // Act
        var result = project.Progress;

        // Assert
        Assert.Equal(100, result);
    }

}