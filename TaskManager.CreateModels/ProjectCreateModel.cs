using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.CreateModels
{
    public class ProjectCreateModel
    {
        public string Name { get; }
        public string Description { get; }
        public ProjectType ProjectType { get; }

        public ProjectCreateModel(string name, string description, ProjectType projectType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва проєкту не може бути порожньою", nameof(name));

            Name = name;
            Description = description;
            ProjectType = projectType;
        }
    }
}