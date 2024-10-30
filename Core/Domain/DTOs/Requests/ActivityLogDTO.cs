namespace Core.Domain.DTOs.Requests {
    public class ActivityLogDTO {
        public long id { get; set; }
        public long actionTime { get; set; }
        public string actionDetails { get; set; }
        public string actionCategory { get; set; }
        public string username { get; set; }
        public string externalID { get; set; }
    }
}
