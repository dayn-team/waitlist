using Core.Domain.Enums;

namespace Core.Domain.ValueObjects {
    public class Participant {
        public string phone { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public bool isConsent { get; set; } = false;
        public Gender gender { get; set; }
    }
}
