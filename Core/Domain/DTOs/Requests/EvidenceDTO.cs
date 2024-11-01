using Core.Domain.Enums;

namespace Core.Domain.DTOs.Requests
{
    public class EvidenceDTO
    {
        public string userId { get; set; }
        public string transactionId { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public EvidenceType type { get; set; }
    }
}
