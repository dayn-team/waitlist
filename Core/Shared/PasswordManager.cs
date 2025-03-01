using Core.Domain.DTOs.Configurations;
using Microsoft.Extensions.Options;

namespace Core.Shared {
    public class PasswordManager {
        private readonly KeySalt passwordSalt;
        public PasswordManager(KeySalt config) {
            passwordSalt = config;
        }
        public string getDigest(string raw) {
            KeySalt aKey = this.passwordSalt;
            string rawData = string.Concat(aKey.salt.Substring(0, aKey.saltIndex), raw, aKey.salt.Substring(aKey.saltIndex, aKey.salt.Length - aKey.saltIndex));
            return Cryptography.Hash.getHash(rawData);
        }
        public string getDigest(string raw, string enckey) {
            if (enckey == null)
                return null;
            string rawData = string.Concat(enckey, raw);
            return Cryptography.Hash.getHash(rawData);
        }
    }
}
