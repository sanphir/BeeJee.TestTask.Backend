namespace BeeJee.TestTask.Backend.Dto
{
    public class TasksResponseDto
    {
        public IEnumerable<TaskDto> Tasks { get; set; }
        public int total_task_count { get; set; }
    }
}
