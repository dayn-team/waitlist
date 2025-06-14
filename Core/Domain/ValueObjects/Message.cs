using Core.Shared;

namespace Core.Domain.ValueObjects {
    public class Message {
        public string MessageID { get; set; } = Cryptography.CharGenerator.genID();
        public string Details { get; set; }
        public long TransDate { get; set; }
        public bool Resolved { get; set; }
        public Participant Creator { get; set; }
        public List<Message> Responses { get; set; }
    }
}
