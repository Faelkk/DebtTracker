using Amazon.DynamoDBv2.DataModel;

namespace DebtTrack.Models;

[DynamoDBTable("Payments")]
public class PaymentModel
{
    [DynamoDBHashKey]
    public string UserId { get; set; } = default!;

    [DynamoDBRangeKey]
    public string PaymentId { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty]
    public string DebtId { get; set; } = default!;

    [DynamoDBProperty]
    public string InstallmentId { get; set; } = default!;

    [DynamoDBProperty]
    public decimal Amount { get; set; }

    [DynamoDBProperty]
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}
