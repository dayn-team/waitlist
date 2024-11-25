using Core.Shared;

namespace Core.Domain.ValueObjects {
    public class Message {
        public string messageID { get; set; } = Cryptography.CharGenerator.genID();
        public string details { get; set; }
        public long transDate { get; set; }
        public bool resolved { get; set; }
        public Participant creator { get; set; }
        public List<Message> responses { get; set; }
    }
}
