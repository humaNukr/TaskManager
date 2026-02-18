using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.TaskManager.Tests.Mappers
{
    public class TaskMapperTest
    {
        [Fact]
        public void MapToUI_ValidData_ReturnsCorrectUIModel()
        {
            var data = new TaskDataModel(
                Guid.NewGuid(),
                "Тестове завдання",
                "Опис",
                TaskPriority.High,
                DateTimeOffset.Now.AddDays(1),
                false
            );

            var uiModel = TaskMapper.MapToUI(data);
            Assert.NotNull(uiModel);
            Assert.NotEqual(Guid.Empty, uiModel.Id);
            Assert.Equal(data.ProjectId, uiModel.ProjectId);
            Assert.Equal(data.Name, uiModel.Name);
            Assert.Equal(data.Description, uiModel.Description);
            Assert.Equal(data.Priority, uiModel.Priority);
            Assert.Equal(data.DueDate, uiModel.DueDate);
            Assert.Equal(data.IsCompleted, uiModel.IsCompleted);
        }
    }
}
