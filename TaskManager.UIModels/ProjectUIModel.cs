using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.UIModels
{
    public class ProjectUIModel
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectType ProjectType { get; set; }
        public int TotalTasksCount { get; set; }
        public int CompletedTasksCount { get; set; }

        // обчислюється динамічно — не зберігається в полі,
        // щоб завжди відповідати актуальним TotalTasksCount / CompletedTasksCount
        public double Progress
        {
            get
            {
                if (TotalTasksCount == 0)
                {
                    return 0; // захист від ділення на нуль
                }
                return (double)CompletedTasksCount / TotalTasksCount * 100;
            }
        }
        public ProjectUIModel(Guid id, string name, string description, ProjectType projectType, int totalTasksCount, int completedTasksCount)
        {
            Id = id;
            Name = name;
            Description = description;
            ProjectType = projectType;
            TotalTasksCount = totalTasksCount;
            CompletedTasksCount = completedTasksCount;
        }

        public override string ToString()
        {
            // іконка підбирається за типом проєкту для зручного відображення у списку
            string categoryIcon = ProjectType switch
            {
                ProjectType.Educational => "🎓",
                ProjectType.Work => "💼",
                ProjectType.Personal => "🏠",
                ProjectType.Research => "🔬",
                ProjectType.Hobby => "🎨",
                ProjectType.Volunteer => "🤝",
                _ => "📁"
            };

            return $"{categoryIcon} {Name} ({Progress:F1}% виконано)";
        }
    }
}
