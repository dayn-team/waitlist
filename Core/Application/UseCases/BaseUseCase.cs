using Core.Application.Errors;
using Core.Application.Interfaces.Infrastructure.Cache;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Domain.DTOs.Configurations;
using Core.Domain.DTOs.Others;
using Core.Domain.Enums;
using Core.Shared;
using Microsoft.Extensions.Options;

namespace Core.Application.UseCases {
    public class BaseUseCase {
        private readonly SystemVariables _sysVar;
        private readonly ICacheService _cacheService;
        private readonly IIdentityManager _identity;
        protected IdentityData profile;
        public BaseUseCase(IOptionsMonitor<SystemVariables> config, ICacheService cacheService, IIdentityManager identity) {
            _sysVar = config.CurrentValue;
            _identity = identity;
            _cacheService = cacheService;
        }

        protected async Task<string> saveSession(AccountType type, string userID, string? pkey = null, int timeout = 1440, int slidExp = 60) {
            if (_sysVar.debug) {
                timeout = 14400;
                slidExp = 14400;
            }
            string key = $"{type.ToString()}_{userID}_lgnpbdc";
            pkey = pkey ?? Cryptography.CharGenerator.genID();
            await _cacheService.addWithKey(key, pkey, timeout, slidExp);
            return pkey;
        }

        public async Task verifySession(bool verifyKey = true, bool enforceCompleteLogin = true) {
            if (!_identity.valid)
                throw new AuthenticationError(_identity.message);
            this.profile = _identity.getProfile<IdentityData>();
            if (!verifyKey)
                return;
            string key = $"{profile.getAccountType().ToString()}_{profile.username}_lgnpbdc";
            string? pkey = await _cacheService.getWithKey(key);
            if (pkey != profile.publicKey)
                throw new AuthenticationError($"Session is now invalid. Please login again");
            if (enforceCompleteLogin)
                if (profile.loginComplete != 1)
                    throw new AuthenticationError("An action is required on this Account. Kindly complete action to continue");
        }
    }
}
