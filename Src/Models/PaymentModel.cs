using Amazon.DynamoDBv2.DataModel;

namespace DebtTrack.Models;

[DynamoDBTable("Payments")]
public class PaymentModel
{
    [DynamoDBHashKey] // Partition Key
    public string PaymentId { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty]
    public string DebtId { get; set; }

    [DynamoDBProperty]
    public string InstallmentId { get; set; } 

    [DynamoDBProperty]
    public decimal Amount { get; set; }

    [DynamoDBProperty]
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}