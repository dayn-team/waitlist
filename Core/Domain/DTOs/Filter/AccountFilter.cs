using Core.Domain.Enums;

namespace Core.Domain.DTOs.Filter {
    public class AccountFilter : Filter {
        public string username {
            get {
                string thisName = GetCallerName();
                return getValue<string>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue(value, thisName);
            }
        }

        public string password {
            get {
                string thisName = GetCallerName();
                return getValue<string>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue(value, thisName);
            }
        }

        public string phone {
            get {
                string thisName = GetCallerName();
                return getValue<string>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue(value, thisName);
            }
        }
        public string email {
            get {
                string thisName = GetCallerName();
                return getValue<string>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue(value, thisName);
            }
        }
        public string externalID {
            get {
                string thisName = GetCallerName();
                return getValue<string>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue(value, thisName);
            }
        }

        public string publicKey {
            get {
                string thisName = GetCallerName();
                return getValue<string>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue(value, thisName);
            }
        }

        public AccountType type {
            get {
                string thisName = GetCallerName();
                return getValue<AccountType>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue<AccountType>(value, thisName);
            }
        }

        public Privilege privilege {
            get {
                string thisName = GetCallerName();
                return getValue<Privilege>(thisName);
            }
            set {
                string thisName = GetCallerName();
                setValue<Privilege>(value, thisName);
            }
        }
    }
}
