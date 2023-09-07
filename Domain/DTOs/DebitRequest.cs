using Newtonsoft.Json;

namespace Domain;

public class DebitRequest
{
    [JsonProperty("principalDebitAccount")]
    public string? PrincipalDebitAccount { get; set; }

    [JsonProperty("principalCreditAccount")]
    public string? PrincipalCreditAccount { get; set; }

    [JsonProperty("principalAmount")]
    public int PrincipalAmount { get; set; }

    [JsonProperty("feeAmount")]
    public int FeeAmount { get; set; }

    [JsonProperty("vatDebitAccount")]
    public string? VatDebitAccount { get; set; }

    [JsonProperty("vatCreditAccount")]
    public string? VatCreditAccount { get; set; }

    [JsonProperty("vatAmount")]
    public int VatAmount { get; set; }

    [JsonProperty("debitCurrency")]
    public string? DebitCurrency { get; set; }

    [JsonProperty("creditCurrency")]
    public string? CreditCurrency { get; set; }

    [JsonProperty("transactionDebitType")]
    public int TransactionDebitType { get; set; }

    [JsonProperty("channelID")]
    public int ChannelID { get; set; }

    [JsonProperty("transactionNarration")]
    public string? TransactionNarration { get; set; }

    [JsonProperty("transactionReference")]
    public string? TransactionReference { get; set; }

    [JsonProperty("transactionFeeCode")]
    public int TransactionFeeCode { get; set; }

    [JsonProperty("ftCommissionTypes")]
    public string? FtCommissionTypes { get; set; }
}