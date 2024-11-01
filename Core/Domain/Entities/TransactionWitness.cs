
using Core.Domain.Enums;

using Core.Shared;


namespace Core.Domain.Entities;
public class TransactionWitness : BaseEntity
{
    public TransactionWitness(string userId, string transactionId)
    {
        this.userId = userId;
        this.transactionId = transactionId;
        this.createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
    }

    public string userId { get; set; }
    public User? user { get; set; }
    public string transactionId { get; set; }
    public Transaction? transaction { get; set; }

    public long createdOn { get; set; }

}