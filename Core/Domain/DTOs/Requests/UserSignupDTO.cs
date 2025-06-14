using Core.Domain.Enums;
using Core.Domain.ValueObjects;

namespace Core.Domain.DTOs.Requests {
    public class UserSignupDTO {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public Privilege Privilege { get; set; }
        public AccountType Type { get; set; }
        public Country Country { get; set; }
    }
}
