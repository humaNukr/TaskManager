using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.TaskManager.Tests.Mappers;

public class TaskMapperTest
{
    private readonly TaskMapper _mapper;

    public TaskMapperTest()
    {
        _mapper = new TaskMapper();
    }

    [Fact]
    public void MapToUI_ValidData_ReturnsCorrectUIModel()
    {
        // Arrange
        var data = new TaskDataModel(Guid.NewGuid(), "Task", "Desc", TaskPriority.Low, DateTimeOffset.Now, false);

        // Act
        var result = _mapper.MapToUI(data);

        //Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(data.ProjectId, result.ProjectId);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Description, result.Description);
        Assert.Equal(data.Priority, result.Priority);
        Assert.Equal(data.DueDate, result.DueDate);
        Assert.Equal(data.IsCompleted, result.IsCompleted);
    }
}
