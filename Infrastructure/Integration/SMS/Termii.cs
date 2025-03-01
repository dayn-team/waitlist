using Core.Application.Interfaces.Infrastructure.SMS;
using Core.Domain.DTOs.Configurations;
using Core.Shared;
using Infrastructure.Abstraction.HTTP;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Infrastructure.Integration.SMS {
    [RegisterAsSingleton]
    public class Termii : ISMSClient {
        private readonly TermiiConfig config;
        private readonly IHttpClient requestor;
        private readonly SystemVariables _sysVar;
        public Termii(IOptionsMonitor<SystemVariables> config, IHttpClient requestor) {
            this.config = config.CurrentValue.Termii;
            this.requestor = requestor;
            this._sysVar = config.CurrentValue;
        }
        public Termii(TermiiConfig config, IHttpClient requestor) {
            this.config = config;
            this.requestor = requestor;
        }

        public async Task<bool> sendSMS(string message, string phone) {
            JObject requestBody = new JObject();
            requestBody.Add("api_key", config.api_key);
            requestBody.Add("sms", message);
            requestBody.Add("to", phone);
            requestBody.Add("from", config.senderID);
            requestBody.Add("channel", "dnd");
            requestBody.Add("type", "plain");
            Dictionary<string, string> headers = new Dictionary<string, string>();
            string url = string.Concat(config.root, config.sendSMS);
            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
            var r = await requestor.sendRequest(url, HttpMethod.Post, content, headers);
            if (r?.httpStatus == 200)
                return true;
            return false;
        }

        public async Task<string> sendOTP(string phone) {
            if (_sysVar.debug) {
                return $"stgk_{Cryptography.CharGenerator.genID()}";
            }
            JObject requestBody = new JObject();
            requestBody.Add("api_key", config.api_key);
            requestBody.Add("message_type", "NUMERIC");
            requestBody.Add("to", phone);
            requestBody.Add("from", "N-Alert");
            requestBody.Add("channel", "dnd");
            requestBody.Add("pin_attempts", 5);
            requestBody.Add("pin_time_to_live", 10);
            requestBody.Add("pin_length", 4);
            requestBody.Add("pin_placeholder", "<1234>");
            requestBody.Add("message_text", "Your TAP Confirmation code is <1234>");
            requestBody.Add("pin_type", "NUMERIC");
            string url = string.Concat(config.root, config.sendOTP);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
            var r = await requestor.sendRequest(url, HttpMethod.Post, content, headers);
            if (r?.httpStatus == 200) {
                var response = JObject.Parse(r.result).ToObject<TermiiOTPResponse>();
                return response.pinId;
            }
            return null;
        }

        public async Task<bool> verifyOTP(string phone, string otp, string token) {
            if (_sysVar.debug) {
                if (token.StartsWith("stgk_")) {
                    if (otp != "0176")
                        return false;
                    return true;
                }
            }
            JObject requestBody = new JObject();
            requestBody.Add("api_key", config.api_key);
            requestBody.Add("pin_id", token);
            requestBody.Add("pin", otp);
            string url = string.Concat(config.root, config.verify);
            string contentType = "application/json";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, contentType);
            var r = await requestor.sendRequest(url, HttpMethod.Post, content, headers);
            if (r?.httpStatus == 200) {
                var responseObj = JObject.Parse(r.result).ToObject<TermiiOTPVerify>();
                return responseObj.verified;
            }
            return false;
        }
    }

    public class TermiiOTPResponse {
        public string pinId { get; set; }
        public string to { get; set; }
        public string verifysmsStatus { get; set; }
    }
    public class TermiiOTPVerify {
        public string pinId { get; set; }
        public bool verified { get; set; }
    }
}
