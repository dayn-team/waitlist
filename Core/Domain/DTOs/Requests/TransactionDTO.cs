using Core.Domain.Enums;
using Core.Domain.ValueObjects;

namespace Core.Domain.DTOs.Requests {
    public class TransactionDTO {
        public DateTime LoanDate { get; set; }
        public double Amount { get; set; }
        public DateTime PaybackDate { get; set; }
        public Participant OtherParty { get; set; }
        public List<Participant> Witnesses { get; set; }
        public PaymentPlan PaymentPlan { get; set; }
        public CreatorClass? CreatedBy { get; set; }
    }
}
