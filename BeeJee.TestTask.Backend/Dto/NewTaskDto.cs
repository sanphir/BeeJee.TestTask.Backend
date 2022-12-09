using System.Text.Json.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public class NewTaskDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
