using Newtonsoft.Json;

namespace Domain;

public class DebitResponse
{
    [JsonProperty("content")]
    public Content? Content { get; set; }

    [JsonProperty("error")]
    public Error? Error { get; set; }

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

public class Content
{
    [JsonProperty("principalFTResponse")]
    public string? PrincipalFTResponse { get; set; }

    [JsonProperty("vatFTResponse")]
    public string? VatFTResponse { get; set; }

    [JsonProperty("feeFTResponse")]
    public string? FeeFTResponse { get; set; }
}

public class Error
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }
}