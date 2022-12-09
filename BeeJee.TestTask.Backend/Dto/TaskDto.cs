using BeeJee.TestTask.DAL.Models;
using System.Text.Json.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public class TaskDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("status")]
        public TaskModelStatus Status { get; set; }
    }
}
