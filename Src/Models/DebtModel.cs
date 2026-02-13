using Amazon.DynamoDBv2.DataModel;

namespace DebtTrack.Models;

[DynamoDBTable("Debts")]
public class DebtModel
{
    [DynamoDBHashKey]
    public string UserId { get; set; } = default!;

    [DynamoDBRangeKey]
    public string DebtId { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty]
    public bool IsMyDebt { get; set; }

    [DynamoDBProperty]
    public string InvolvedPartyName { get; set; } = default!;

    [DynamoDBProperty]
    public string Description { get; set; } = default!;

    [DynamoDBProperty]
    public decimal TotalAmount { get; set; }

    [DynamoDBProperty]
    public int Installments { get; set; }

    [DynamoDBProperty]
    public decimal InstallmentValue { get; set; }

    [DynamoDBProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DynamoDBProperty]
    public DateTime DueDate { get; set; }

    [DynamoDBProperty]
    public bool IsPaid { get; set; } = false;
}



