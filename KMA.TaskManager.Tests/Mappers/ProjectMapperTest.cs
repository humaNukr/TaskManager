using System;
using System.Collections.Generic;
using KMA.TaskManager.Common.Enums;
using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Mappers;
using Xunit;

namespace KMA.TaskManager.Tests.Mappers;

public class ProjectMapperTest
{
    private readonly ProjectMapper _mapper;

    public ProjectMapperTest()
    {
        _mapper = new ProjectMapper();
    }

    [Fact]
    public void MapToListDTO_ValidDataModel_ReturnsCorrectDTO()
    {
        // Arrange
        var data = new ProjectDataModel("Test Project", "Description", ProjectType.Personal);
        int total = 10;
        int completed = 5;

        // Act
        var result = _mapper.MapToListDTO(data, total, completed);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Id, result.Id);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(total, result.TotalTasks);
        Assert.Equal(completed, result.CompletedTasks);
    }

    [Fact]
    public void MapToDetailsDTO_ValidDataModel_ReturnsCorrectDTO()
    {
        // Arrange
        var data = new ProjectDataModel("Test Project", "Description", ProjectType.Personal);
        var tasks = new List<TaskListDTO>
        {
            new TaskListDTO(Guid.NewGuid(), "Task 1", TaskPriority.Medium, false, false)
        };

        // Act
        var result = _mapper.MapToDetailsDTO(data, tasks);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(data.Id, result.Id);
        Assert.Equal(data.Name, result.Name);
        Assert.Equal(data.Description, result.Description);
        Assert.Equal(data.ProjectType, result.ProjectType);
        Assert.Single(result.Tasks);
    }

    [Fact]
    public void MapToData_ValidCreateModel_ReturnsCorrectDataModel()
    {
        // Arrange
        var createModel = new ProjectCreateModel("New Project", "New Desc", ProjectType.Work);

        // Act
        var result = _mapper.MapToData(createModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createModel.Name, result.Name);
        Assert.Equal(createModel.Description, result.Description);
        Assert.Equal(createModel.ProjectType, result.ProjectType);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public void Mappers_NullInput_ReturnsNull()
    {
        // Assert
        Assert.Null(_mapper.MapToListDTO(null, 0, 0));
        Assert.Null(_mapper.MapToDetailsDTO(null, new List<TaskListDTO>()));
        Assert.Null(_mapper.MapToData(null));
    }
}