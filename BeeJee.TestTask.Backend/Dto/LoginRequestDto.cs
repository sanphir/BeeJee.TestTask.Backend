using System.Text.Json.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public class LoginRequestDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
