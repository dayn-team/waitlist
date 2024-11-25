using Core.Domain.ValueObjects;

namespace Core.Domain.DTOs.Requests {
    public class RepaymentDTO {
        public string transaction { get; set; }
        public Participant payer { get; set; }
        public EvidenceFile paymentEvidence { get; set; }
        public double amountPaid { get; set; }
        public long transDate { get; set; }
        public List<Message> disputeLog { get; set; }
        public bool verified { get; set; }
    }
}
