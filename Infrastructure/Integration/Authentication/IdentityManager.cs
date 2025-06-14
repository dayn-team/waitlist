using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Domain.DTOs.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NetCore.AutoRegisterDi;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Infrastructure.Integration.Authentication {
    [RegisterAsScoped]
    public class IdentityManager : IIdentityManager {
        private readonly SystemVariables _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private protected IHeaderDictionary headers;
        private protected JObject profile;
        public bool Valid { get; private set; }
        public string Message { get; private set; }
        public string IPAddress { get; private set; }
        public string Useragent { get; private set; }
        private readonly JWTIdentity tokenMngr;
        public string EndPointAddress { get; private set; }
        public IdentityManager(IOptionsMonitor<SystemVariables> config, IHttpContextAccessor contextAccessor) {
            _config = config.CurrentValue;
            _contextAccessor = contextAccessor;
            tokenMngr = new JWTIdentity(_config.JWTSettings.jwtSecret);
            loadProfile(_config.KeyExpires);
            getIPAddress();
            getRoute();
            getUserAgent();
        }
        private void getRoute() {
            try {
                EndPointAddress = _contextAccessor.HttpContext.Request.Path;
            } catch { }
        }
        private void getUserAgent() {
            try {
                Useragent = GetHeaderValue("User-Agent").ToString();
            } catch { }
        }
        private void getIPAddress() {
            string[] ipAddresses = new string[] { null };
            string altIPAddress = null;
            try {
                try {
                    ipAddresses = GetHeaderValue("X-Forwarded-For").ToString().Split(':');
                } catch { }
                altIPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            } catch { }
            var ipAddress = string.IsNullOrEmpty(ipAddresses[0]) ? altIPAddress : ipAddresses[0];
            IPAddress = ipAddress;
        }
        public void LoadCustomHeaders(IDictionary<string, object> header) {
            var headersUsable = new Dictionary<string, StringValues>();
            foreach (var kvp in header) {
                try {
                    try {
                        headersUsable.Add(kvp.Key, (StringValues)Encoding.Default.GetString((byte[])kvp.Value));
                    } catch {
                        var vstr = new StringValues((string)kvp.Value);
                        headersUsable.Add(kvp.Key, vstr);
                    }
                } catch {
                    throw;
                }
            }
            var headers = new HeaderDictionary(headersUsable) as IHeaderDictionary;
            this.headers = headers;
            loadProfile(_config.KeyExpires);
        }
        public bool SessionValid() {
            loadProfile(_config.KeyExpires);
            return Valid;
        }

        private void loadProfile(bool enforceExpiryCheck = true) {
            try {
                headers = headers ?? _contextAccessor.HttpContext.Request.Headers;
                string h = headers["Authorization"];
                if (!string.IsNullOrEmpty(h)) {
                    string[] auths = h.Trim().Split(" ");
                    if (auths.Length >= 2) {
                        h = auths[^1];
                        profile = tokenMngr.verifyToken(h, enforceExpiryCheck ? enforceExpiryCheck : _config.JWTSettings.identityExpires);
                        Valid = profile != null;
                        if (!Valid)
                            Message = "The authentication is invalid or expired";
                    } else { Valid = false; Message = "Authorization Header is not valid. Login again or contact admin"; }
                } else { Valid = false; Message = "Authorization Header is missing in request"; }
            } catch {
                Valid = false;
                Message = "Authorization Header is missing in request";
            }
        }
        public string GetJWTIdentity(Dictionary<string, string> identity, int expiry = 0) {
            return tokenMngr.getToken(identity, expiry < 1 ? _config.JWTSettings.identityExpiryMins : expiry);
        }
        public string GetHeaderValue(string key) {
            StringValues p;
            headers.TryGetValue(key, out p);
            return p;
        }
        public IDictionary<string, object> GetAllHeader() {
            var comparer = StringComparer.OrdinalIgnoreCase;
            IDictionary<string, object> snew = new Dictionary<string, object>(comparer);
            foreach (KeyValuePair<string, StringValues> val in headers) {
                snew.Add(val.Key, val.Value.ToString());
            }
            return snew;
        }
        public T GetProfile<T>() {
            return profile.ToObject<T>();
        }
    }
}
