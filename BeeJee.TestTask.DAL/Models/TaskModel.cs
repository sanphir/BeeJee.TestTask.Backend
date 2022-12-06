using System.ComponentModel.DataAnnotations;

namespace BeeJee.TestTask.DAL.Models
{
    public enum TaskModelStatus
    {
        NotCompleted = 0,
        NotCompletedAndEditedByAdmin = 1,
        Completed = 10,
        CompletedAndEditedByAdmin = 11
    }

    public class TaskModel
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public TaskModelStatus Status { get; set; }
    }
}
