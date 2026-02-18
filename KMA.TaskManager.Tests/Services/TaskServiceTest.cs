using KMA.TaskManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.TaskManager.Tests.Services
{
    public class TaskServiceTest
    {
        [Fact]
        public void GetTasksByProjectId_ForBakeryProject_ReturnsTenTasks()
        {

            var service = new TaskService();
            var bakeryId = MockStorage.Projects.First(p => p.Name.Contains("пекарні")).Id;
            var tasks = service.GetTasksByProjectId(bakeryId);

            Assert.Equal(10, tasks.Count);
            Assert.All(tasks, t => Assert.Equal(bakeryId, t.ProjectId));
        }

        [Fact]
        public void GetTaskById_WhenIdExists_ReturnsCorrectTask()
        {
            var service = new TaskService();      
            var expectedTask = MockStorage.Tasks.First();
            var result = service.GetTaskById(expectedTask.Id);

            Assert.NotNull(result);
            Assert.Equal(expectedTask.Name, result.Name);
        }
    }
}
