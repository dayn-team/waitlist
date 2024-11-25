using Core.Domain.Enums;
using Core.Domain.ValueObjects;

namespace Core.Domain.DTOs.Requests {
    public class TransactionDTO {
        public long loanDate { get; set; }
        public double amount { get; set; }
        public long paybackDate { get; set; }
        public Participant otherParty { get; set; }
        public List<Participant> witnesses { get; set; }
        public PaymentPlan paymentPlan { get; set; }
        public CreatorClass? createdBy { get; set; }
    }
}
