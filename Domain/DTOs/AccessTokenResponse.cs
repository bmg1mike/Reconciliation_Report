using Newtonsoft.Json;

namespace Domain.DTOs
{
    public class AccessTokenResponse
    {
        [JsonProperty("content")]
        public TokenContent? Content { get; set; }

        [JsonProperty("error")]
        public object? Error { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("requestId")]
        public string? RequestId { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("requestTime")]
        public DateTime RequestTime { get; set; }

        [JsonProperty("responseTime")]
        public DateTime ResponseTime { get; set; }
    }
    public class TokenContent
    {
        [JsonProperty("bearerToken")]
        public string? BearerToken { get; set; }

        [JsonProperty("expiryTime")]
        public DateTime ExpiryTime { get; set; }
    }
}