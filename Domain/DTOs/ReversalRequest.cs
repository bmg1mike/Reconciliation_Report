using Newtonsoft.Json;

namespace Domain;

public class ReversalRequest
{
    [JsonProperty("ftReference")]
    public string? FtReference { get; set; }

    [JsonProperty("branchCode")]
    public string? BranchCode { get; set; }

    [JsonProperty("applicationName")]
    public string? ApplicationName { get; set; }
}