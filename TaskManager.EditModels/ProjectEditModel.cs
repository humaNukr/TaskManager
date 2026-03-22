using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.EditModels;

public class ProjectEditModel
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ProjectType ProjectType { get; }

    public ProjectEditModel(
        Guid id,
        string name,
        string description,
        ProjectType projectType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва проєкту не може бути порожньою", nameof(name));

        Id = id;
        Name = name;
        Description = description;
        ProjectType = projectType;
    }
}