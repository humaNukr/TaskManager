using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Common.Enums;

namespace KMA.TaskManager.DataModels
{
    public class ProjectDataModel
    {
        public Guid Id { get; } // тільки get — Id не можна змінити після створення

        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectType ProjectType { get; }

        public ProjectDataModel()
        {
        }

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
}