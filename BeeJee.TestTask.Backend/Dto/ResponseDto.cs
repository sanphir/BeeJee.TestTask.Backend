using System.Text.Json.Serialization;

namespace BeeJee.TestTask.Backend.Dto
{
    public enum ResponseStatus
    {
        Ok,
        Error
    }

    public class ResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; private set; }
        public ResponseDto(ResponseStatus status)
        {
            Status = status.ToString();
        }
    }

    public class ResponseMessageDto<T> : ResponseDto where T : class
    {
        /// <summary>
        /// Response data
        /// </summary>       
        [JsonPropertyName("message")]
        public T? Message { get; set; }

        public ResponseMessageDto(ResponseStatus status) : base(status)
        {
        }
    }
}
