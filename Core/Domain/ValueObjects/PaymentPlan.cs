using Core.Domain.Enums;

namespace Core.Domain.ValueObjects {
    public class PaymentPlan {
        public double payment { get; set; }
        public int duration { get; set; }
        public PaymentFrequency frequency { get; set; } 
    }
}
