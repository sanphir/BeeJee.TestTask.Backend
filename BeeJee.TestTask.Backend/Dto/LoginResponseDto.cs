using System.Text.Json.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public class LoginResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
