using Core.Domain.Enums;
using Core.Shared;

namespace Core.Domain.DTOs.Others {
    public class IdentityData {
        public string username { get; set; }
        public string fullname { get; set; }
        public string publicKey { get; set; }
        public long dateIssued { get; set; } = Utilities.getTodayDate().unixTimestamp;
        public int? accountType { get; set; }
        public int? accountPrivilege { get; set; }
        public string id { get; set; }
        public int status { get; set; }
        public string device { get; set; }
        public string externalID { get; set; }
        public int loginComplete { get; set; }
        public int pwca { get; set; }
        public int tfaa { get; set; }
        public int tfaen { get; set; }

        public AccountType getAccountType() {
            if (accountType == null)
                return default(AccountType);
            return (AccountType)(int)accountType;
        }

        public Privilege getAccountPrivilege() {
            if (accountPrivilege == null)
                return default(Privilege);
            return (Privilege)(int)accountPrivilege;
        }

        public Dictionary<string, string> getTokenObj() {
            Dictionary<string, string> tokenObj = new Dictionary<string, string>();
            tokenObj.Add("username", username.ToLower());
            tokenObj.Add("id", id);
            tokenObj.Add("fullname", fullname);
            tokenObj.Add("dateIssued", Utilities.getTodayDate().unixTimestamp.ToString());
            tokenObj.Add("accountPrivilege", (accountPrivilege).ToString());
            tokenObj.Add("accountType", ((int)accountType).ToString());
            tokenObj.Add("publicKey", publicKey);
            tokenObj.Add("device", device);
            return tokenObj;
        }
    }
}
