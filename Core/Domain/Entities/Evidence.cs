
using Core.Domain.Enums;

using Core.Shared;


namespace Core.Domain.Entities;
public class Evidence : BaseEntity
{
    public Evidence(string userId, string transactionId, string name, string url, EvidenceType type)
    {
        this.userId = userId;
        this.transactionId = transactionId;
        this.name = name;
        this.url = url;
        this.type = type;
        this.createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
    }

    public string userId { get; set; }
    public User? user { get; set; }
    public string transactionId { get; set; }
    public Transaction? transaction { get; set; }
    public string name { get; set; }
    public EvidenceType type { get; set; }
    public string url { get; set; }

    public long createdOn { get; set; }

}