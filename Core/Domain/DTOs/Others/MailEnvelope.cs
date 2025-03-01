namespace Core.Domain.DTOs.Others {
    public class MailEnvelope {
        public string[] toName { get; set; }
        public string[] toAddress { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
        public string[] attachment { get; set; }
        public bool bodyIsPlainText { get; set; } = false;
        public List<Attachment> attachmentObj { get; set; }
    }

    public class Attachment {
        public byte[] rawAttachment { get; set; }
        public string fileUrl { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
        public string format { get; set; }
    }
}
