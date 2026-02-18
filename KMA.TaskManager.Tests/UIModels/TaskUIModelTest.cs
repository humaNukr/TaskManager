using KMA.TaskManager.UIModels;
using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.Tests.UIModels
{
    public class TaskUIModelTest
    {
        [Fact]
        public void IsOverdue_WhenPastDateAndNotCompleted_ReturnsTrue()
        {
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

            bool result = task.IsOverdue;

            Assert.True(result);
        }

        [Fact]
        public void IsOverdue_WhenPastDateButCompleted_ReturnsFalse()
        {
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

            bool result = task.IsOverdue;

            Assert.False(result);
        }

        [Fact]
        public void IsOverdue_WhenFutureDate_ReturnsFalse()
        {
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

            bool result = task.IsOverdue;

            Assert.False(result);
        }
    }
}
