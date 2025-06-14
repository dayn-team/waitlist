using Core.Domain.Enums;
using Core.Shared;

namespace Core.Domain.DTOs.Others {
    public class IdentityData {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string PublicKey { get; set; }
        public DateTime DateIssued { get; set; } = DateTime.Now;
        public int? AccountType { get; set; }
        public int? AccountPrivilege { get; set; }
        public string Id { get; set; }
        public int Status { get; set; }
        public string Device { get; set; }
        public string ExternalID { get; set; }
        public int LoginComplete { get; set; }
        public int Pwca { get; set; }
        public int Tfaa { get; set; }
        public int Tfaen { get; set; }

        public AccountType GetAccountType() {
            if (AccountType == null)
                return default(AccountType);
            return (AccountType)(int)AccountType;
        }

        public Privilege GetAccountPrivilege() {
            if (AccountPrivilege == null)
                return default(Privilege);
            return (Privilege)(int)AccountPrivilege;
        }

        public Dictionary<string, string> GetTokenObj() {
            Dictionary<string, string> tokenObj = new Dictionary<string, string>();
            tokenObj.Add("username", Username.ToLower());
            tokenObj.Add("id", Id);
            tokenObj.Add("fullname", Fullname);
            tokenObj.Add("dateIssued", Utilities.GetTodayDate().unixTimestamp.ToString());
            tokenObj.Add("accountPrivilege", (AccountPrivilege).ToString());
            tokenObj.Add("accountType", ((int)AccountType).ToString());
            tokenObj.Add("publicKey", PublicKey);
            tokenObj.Add("device", Device);
            return tokenObj;
        }
    }
}
