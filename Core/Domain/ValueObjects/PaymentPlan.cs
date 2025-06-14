using Core.Domain.Enums;

namespace Core.Domain.ValueObjects {
    public class PaymentPlan {
        public double Payment { get; set; }
        public int Duration { get; set; }
        public PaymentFrequency Frequency { get; set; } 
    }
}
