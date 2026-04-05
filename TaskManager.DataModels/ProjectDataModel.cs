using System;
using KMA.TaskManager.Common.Enums;
using SQLite;

namespace KMA.TaskManager.DataModels;

public class ProjectDataModel
{
    [PrimaryKey]
    public Guid Id { get; set; } // тільки get — Id не можна змінити після створення
    public string Name { get; set; }
    public string Description { get; set; }
    public ProjectType ProjectType { get; set; }

    public ProjectDataModel() { }

    public ProjectDataModel(string name, string description, ProjectType projectType)
        : this(Guid.NewGuid(), name, description, projectType)
    {
    }

    public ProjectDataModel(Guid id, string name, string description, ProjectType projectType)
    {
        Id = id;
        Name = name;
        Description = description;
        ProjectType = projectType;
    }
}