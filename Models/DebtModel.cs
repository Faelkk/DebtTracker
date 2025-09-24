using Amazon.DynamoDBv2.DataModel;

namespace DebtTrack.Models;

[DynamoDBTable("Debts")]
public class DebtModel
{
    [DynamoDBHashKey] 
    public string DebtId { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty]
    public string UserId { get; set; }

    [DynamoDBProperty]
    public string Description { get; set; }

    [DynamoDBProperty]
    public decimal TotalAmount { get; set; }

    [DynamoDBProperty]
    public int? Installments { get; set; }

    [DynamoDBProperty]
    public decimal? InstallmentValue { get; set; }

    [DynamoDBProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DynamoDBProperty]
    public DateTime DueDate { get; set; }

    [DynamoDBProperty]
    public bool IsPaid { get; set; } = false;
}