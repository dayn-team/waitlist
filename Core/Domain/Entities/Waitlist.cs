using Core.Application.Errors;
using Core.Domain.Attributes;
using Core.Shared;

namespace Core.Domain.Entities {
    public class Waitlist : BaseEntity {
        public string Fullname { get; set; }
        [DBIndex(IndexAttributes.UNIQUE)]
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public Waitlist(string fullname, string email) {
            if (!Utilities.isValidName(fullname))
                throw new InputError("Please enter a valid full name");
            if (!Utilities.isEmail(email))
                throw new InputError("A valid email is required");
            this.Email = email;
            this.Fullname = fullname;
            this.DateJoined = DateTime.Now;
        }
    }
}
