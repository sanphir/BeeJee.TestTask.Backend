using System.Text.Json.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public class TasksResponseDto
    {
        [JsonPropertyName("tasks")]
        public IEnumerable<TaskDto> Tasks { get; set; }

        [JsonPropertyName("total_task_count")]
        public int TotalTaskCount { get; set; }
    }
}
