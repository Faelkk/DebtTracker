using Amazon.DynamoDBv2.DataModel;

namespace DebtTrack.Models;

[DynamoDBTable("Installments")]
public class InstallmentModel
{
    [DynamoDBHashKey] 
    public string InstallmentId { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty]
    public string DebtId { get; set; }

    [DynamoDBProperty]
    public int Number { get; set; } 

    [DynamoDBProperty]
    public DateTime DueDate { get; set; }

    [DynamoDBProperty]
    public decimal Amount { get; set; }

    [DynamoDBProperty]
    public decimal PaidAmount { get; set; } = 0;

    [DynamoDBProperty]
    public bool IsPaid { get; set; } = false;
}