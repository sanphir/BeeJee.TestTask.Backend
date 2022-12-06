using BeeJee.TestTask.DAL.Models;

namespace BeeJee.TestTask.Backend.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public TaskModelStatus Status { get; set; }
    }
}
