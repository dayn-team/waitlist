
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;

using Core.Shared;


namespace Core.Domain.Entities;
public class Evidence : BaseEntity
{
    public Evidence(EvidenceDTO dto)
    {
        userId = dto.userId;
        transactionId = dto.transactionId;
        name = dto.name;
        url = dto.url;
        type = dto.type;
        createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
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