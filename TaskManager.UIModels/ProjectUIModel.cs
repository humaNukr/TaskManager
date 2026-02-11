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

        public double Progress
        {
            get
            {
                if (TotalTasksCount == 0)
                {
                    return 0;
                }
                return (double)CompletedTasksCount / TotalTasksCount * 100;
            }
        }
        public ProjectUIModel(string name, string description, ProjectType projectType, int totalTasksCount, int completedTasksCount)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            ProjectType = projectType;
            TotalTasksCount = totalTasksCount;
            CompletedTasksCount = completedTasksCount;
        }
    }
}
