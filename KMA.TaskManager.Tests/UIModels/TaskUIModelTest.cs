using KMA.TaskManager.UIModels;
using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Tests.UIModels
{
    public class TaskUIModelTest
    {
        [Fact]
        public void IsOverdue_WhenPastDateAndNotCompleted_ReturnsTrue()
        {
            //Arrange
            var pastDate = DateTimeOffset.Now.AddDays(-1);
            var task = new TaskUIModel(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Тестове завдання",
                "Опис",
                TaskPriority.High,
                pastDate,
                false
            );

            //Act
            bool result = task.IsOverdue;

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void IsOverdue_WhenPastDateButCompleted_ReturnsFalse()
        {
            //Arrange
            var pastDate = DateTimeOffset.Now.AddDays(-1);
            var task = new TaskUIModel(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Виконане завдання",
                "Опис",
                TaskPriority.Low,
                pastDate,
                true
            );

            //Act
            bool result = task.IsOverdue;

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void IsOverdue_WhenFutureDate_ReturnsFalse()
        {
            //Arrange
            var futureDate = DateTimeOffset.Now.AddDays(1);
            var task = new TaskUIModel(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Завдання на завтра",
                "Опис",
                TaskPriority.Medium,
                futureDate,
                false
            );

            //Act
            bool result = task.IsOverdue;

            //Arrange
            Assert.False(result);
        }
    }
}
