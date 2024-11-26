using Core.Domain.Enums;

namespace Core.Domain.DTOs.Requests {
    public class UserSignupDTO {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public Gender gender { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public Privilege privilege { get; set; }
        public AccountType type { get; set; }
    }
}
