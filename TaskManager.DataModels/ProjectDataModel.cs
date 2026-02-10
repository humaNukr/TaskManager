using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMA.TaskManager.Common.Enums;

namespace TaskManager.DataModels
{
    public class ProjectDataModel
    {
        // id is read-only
        public int Id { get; }

        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectType ProjectType { get; set; }

        public ProjectDataModel(int id, string name, string description, ProjectType projectType)
        {
            Id = id;
            Name = name;
            Description = description;
            ProjectType = projectType;
        }
    }
}
