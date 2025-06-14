using Core.Application.Errors;
using Core.Application.Interfaces.Infrastructure.Cache;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Application.UseCases.Other;
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
        protected IdentityData? profile;
        private readonly CountryDataLoader _countryLoader;
        public BaseUseCase(IOptionsMonitor<SystemVariables> config, ICacheService cacheService, IIdentityManager identity) {
            _sysVar = config.CurrentValue;
            _identity = identity;
            _cacheService = cacheService;
            _countryLoader = CountryDataLoader.Instance;
        }

        public IReadOnlyList<CountryInformation> GetCountryInfo() {
            return _countryLoader.Countries;
        }

        protected async Task<string> SaveSession(AccountType type, string userID, string? pkey = null, int timeout = 1440, int slidExp = 60) {
            if (_sysVar.debug) {
                timeout = 14400;
                slidExp = 14400;
            }
            string key = $"{type.ToString()}_{userID}_lgnpbdc";
            pkey = pkey ?? Cryptography.CharGenerator.genID();
            await _cacheService.AddWithKey(key, pkey, timeout, slidExp);
            return pkey;
        }

        public async Task VerifySession(bool verifyKey = true, bool enforceCompleteLogin = true) {
            if (!_identity.Valid)
                throw new AuthenticationError(_identity.Message);
            this.profile = _identity.GetProfile<IdentityData>();
            if (!verifyKey)
                return;
            string key = $"{profile.GetAccountType().ToString()}_{profile.Username}_lgnpbdc";
            string? pkey = await _cacheService.GetWithKey(key);
            if (pkey != profile.PublicKey)
                throw new AuthenticationError($"Session is now invalid. Please login again");
            if (enforceCompleteLogin)
                if (profile.LoginComplete != 1)
                    throw new AuthenticationError("An action is required on this Account. Kindly complete action to continue");
        }
    }
}
