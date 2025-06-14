using Core.Domain.Enums;

namespace Core.Domain.ValueObjects {
    public class Participant {
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public bool IsConsent { get; set; } = false;
        public Gender Gender { get; set; }
    }
}
