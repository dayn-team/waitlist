using Core.Domain.ValueObjects;

namespace Core.Domain.DTOs.Requests {
    public class DisputeRequest {
        public Message message { get; set; }
        public string paymentID { get; set; }
        public string replyto { get; set; }
    }
}
