using KMA.TaskManager.CreateModels;
using KMA.TaskManager.DataModels;
using KMA.TaskManager.EditModels;
using KMA.TaskManager.Services.DTOModels.Tasks;
using KMA.TaskManager.Services.Interfaces;
using System;

namespace KMA.TaskManager.Services.Mappers
{
    public class TaskMapper : ITaskMapper
    {
        public TaskDataModel MapToData(TaskCreateModel model)
        {
            if (model == null) return null;

            return new TaskDataModel(
                model.ProjectId,
                model.Name,
                model.Description,
                model.Priority,
                model.DueDate,
                false
            );
        }

        public TaskListDTO MapToListDTO(TaskDataModel data)
        {
            if (data == null) return null;

            // Overdue is derived at read time, not persisted as a separate field.
            bool isOverdue = !data.IsCompleted && data.DueDate < DateTimeOffset.Now;

            return new TaskListDTO(
                data.Id,
                data.Name,
                data.Priority,
                data.IsCompleted,
                isOverdue,
                data.DueDate.DateTime
            );
        }

        public TaskDetailsDto MapToDetailsDTO(TaskDataModel data)
        {
            if (data == null) return null;

            bool isOverdue = !data.IsCompleted && data.DueDate < DateTimeOffset.Now;

            return new TaskDetailsDto(
                data.Id,
                data.ProjectId,
                data.Name,
                data.Description,
                data.Priority,
                data.DueDate,
                data.IsCompleted,
                isOverdue
            );
        }

        public void MapUpdateToData(TaskEditModel source, TaskDataModel destination)
        {
            if (source == null || destination == null) return;

            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Priority = source.Priority;
            destination.DueDate = source.DueDate;
            destination.IsCompleted = source.IsCompleted;
        }
    }
}