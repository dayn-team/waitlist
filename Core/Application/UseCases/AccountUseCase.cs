using Core.Application.Errors;
using Core.Application.Interfaces.Email;
using Core.Application.Interfaces.Infrastructure.Cache;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Application.Interfaces.UseCases;
using Core.Domain.DTOs.Configurations;
using Core.Domain.DTOs.Filter;
using Core.Domain.DTOs.Others;
using Core.Domain.DTOs.Requests;
using Core.Domain.DTOs.Response;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Shared;
using Microsoft.Extensions.Options;

namespace Core.Application.UseCases {
    public class AccountUseCase : BaseUseCase, IAccountUseCase {
        private readonly IUserRepository _account;
        private readonly SystemVariables _sysVar;
        private readonly IIdentityManager _idenity;
        private readonly IEmailService _mail;
        private readonly PasswordManager _passManager;
        private readonly ICacheService _cache;
        public AccountUseCase(IUserRepository account, IOptionsMonitor<SystemVariables> sysVar, IIdentityManager idenity, IEmailService email, ICacheService cache) : base(sysVar, cache, idenity) {
            _account = account;
            _sysVar = sysVar.CurrentValue;
            _idenity = idenity;
            _passManager = new PasswordManager(_sysVar.KeySalt);
            _mail = email;
            _cache = cache;
        }
        public async Task<WebResponse<object>> createAccount(UserSignupDTO account) {
            WebResponse response = new WebResponse();           
            await verifyFields(account);
            account.password = _passManager.getDigest(account.password);
            account.privilege = Privilege.SUPERADMIN;
            account.type = AccountType.CUSTOMER;
            User accountObj = new User(account, _passManager.getDigest);
            string key = account.username + "_mlv";
            await _cache.addWithKey(key, Utilities.getTodayDate().unixTimestamp.ToString(), 7200);
            await sendWelcomeEmailVerification(account.username, accountObj.id, account.email);
            await _account.create(accountObj);
            return response.success("Account has been created for you. Kindly check your email for verification link");
        }

        public async Task<WebResponse<object>> verifyAccount(string code) {
            WebResponse response = new WebResponse();
            var key = $"{code}_mlv";
            var d = await _cache.getWithKey(key);
            if (string.IsNullOrEmpty(d))
                throw new InputError("Verification is expired or invalid");
            var account = await _account.get(new AccountFilter { externalID = code });
            if (account == null || account.Count() < 1) {
                throw new InputError("Invalid Profile. This link is not valid");
            }
            await _cache.deleteWithKey(key);
            return response.success("Account has been verified. Thank you");
        }

        private async Task verifyFields(UserSignupDTO account) {
            if (await _account.accountExists(new AccountFilter { username = account.username }))
                throw new LogicError($"{account.username} as a username has been taken");
            if (await _account.accountExists(new AccountFilter { phone = account.phone }))
                throw new LogicError("This phone number cannot be used");
            if (await _account.accountExists(new AccountFilter { email = account.email }))
                throw new LogicError("This email address cannot be used");
        }

        private async Task<bool> sendWelcomeEmailVerification(string username, string urlcode, string email) {
            string content = File.ReadAllText(@"MailTemplates\welcome.html");
            content = content.Replace("{{CustomerName}}", username);
            string url = $"{_sysVar.siteRoot}onboard-verify/{urlcode}";
            content = content.Replace("{{VerificationURL}}", url);
            MailEnvelope mailParam = new MailEnvelope() { subject = $"Welcome to TAP Global Card", body = content, toAddress = new string[] { email }, toName = new string[] { username } };
            return await _mail.send(mailParam);
        }

        public async Task<WebResponse<object>> login(string username, string password) {
            WebResponse response = new WebResponse();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new InputError("Username and password is required");
            string encPassword = _passManager.getDigest(password);
            var account = await _account.get(new AccountFilter { username = username, password = encPassword });
            if (account.Count < 1)
                return response.fail(ResponseCodes.USER_DOES_NOT_EXIST, $"Username and Password match not found");
            if (account[0].status == AccountStatus.DELETED)
                return response.fail(ResponseCodes.USER_DOES_NOT_EXIST);
            if (account[0].status == AccountStatus.SUSPENDED)
                return response.fail(ResponseCodes.ACCESS_DENIED_ERROR, "Account has been suspended. Contact Admin");
            if (account.First().type == AccountType.CUSTOMER) {
                if (account.First().mailVerified) {
                    var code = $"{account[0].id}_mlv";
                    var b = await _cache.getWithKey(code);
                    if (string.IsNullOrEmpty(b)) {
                        await _cache.addWithKey(code, Utilities.getTodayDate().unixTimestamp.ToString(), 7200);
                        await sendWelcomeEmailVerification(account.First().username, account.First().id, account.First().email);
                    }
                    throw new AuthenticationError("Please check your email to confirm your email address");
                }
            }
            bool changePasswordPrompt = false;
            bool setuptfa = false;
            string message = "Authentication Complete";
            int loginComplete = 1;
            string next = string.Empty;
            if (account[0].type != AccountType.CUSTOMER || account[0].tfa == 1) {
                message = "Kindly Complete 2fa";
                next = "2faAuth";
                setuptfa = account[0].tfa == 1 ? false : true;
                if (setuptfa) {
                    message = "Kindly Proceed to setting up your Two Factor Authentication";
                    next = "2faSetup";
                }
                if (account[0].passwordChanged == 0) {
                    changePasswordPrompt = true;
                    message = "Kindly Change your Password to Continue";
                    next = "PasswordChange";
                }
                loginComplete = 0;
            }
            var identityObj = account[0].login(_idenity);
            identityObj.loginComplete = loginComplete;
            identityObj.pwca = changePasswordPrompt ? 1 : 0;
            identityObj.tfaa = setuptfa ? 1 : 0;
            var token = getJWTToken(identityObj);
            string loginDetails = $"Account Login details Username: {username} \n Type: Password Login \n IP: {_idenity.IPAddress} \n useragent : {_idenity.useragent}";
            await _account.loginUpdate(account[0]);
            await saveSession(account[0].type, account[0].username, account[0].publicKey);
            return response.success(message, new { token, profile = identityObj, changePasswordPrompt, setuptfaRequired = setuptfa, next });
        }

        public async Task<WebResponse<object>> updatePassword(string password) {
            WebResponse response = new WebResponse();
            await verifySession(true, false);
            if (profile.pwca == 0) {
                await verifySession();
            }
            if (string.IsNullOrEmpty(password))
                throw new InputError("Invalid Request. Password is required");
            if (password.Length < 6)
                throw new InputError("Password must be at least 6 chars long");
            if (!Utilities.passwordCase(password))
                throw new InputError("Password must contain at least one each of an Uppercase, lowercase, a numeric charcater and a special character");
            string encPassword = _passManager.getDigest(password);
            await _account.updatePassword(encPassword, profile.username);
            if (profile.pwca == 1) {
                profile.pwca = 0;
                var token = getJWTToken(profile);
                return response.success("Token has been updated", new { token });
            }
            return response.success();
        }

        public Task<WebResponse<object>> updateAccount(UserSignupDTO account) {
            throw new NotImplementedException();
        }

        public Task<WebResponse<object>> retrievePassword(string username, string password) {
            throw new NotImplementedException();
        }
        public async Task<WebResponse<object>> resetPassword(string username, string email) {
            WebResponse response = new WebResponse();
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
                throw new InputError("Invalid Request. Username or email is required");
            List<User> user;
            if (!string.IsNullOrEmpty(username)) {
                user = await _account.get(new AccountFilter { username = username });
            } else {
                user = await _account.get(new AccountFilter { email = email });
            }
            if (user.Count < 1)
                throw new InputError("Account was not found");
            string password = Cryptography.CharGenerator.genID(8, Cryptography.CharGenerator.characterSet.HEX_STRING);
            string encPassword = _passManager.getDigest(password);
            await _account.updatePassword(encPassword, user[0].username, 0);
            await sendPasswordResetEmail(user[0].username, password, user[0].email);
            return response.success();
        }

        private string getJWTToken(IdentityData profile) {
            Dictionary<string, string> tokenObj = new Dictionary<string, string> {
                { "username", profile.username },
                { "fullname", profile.fullname },
                { "publicKey", profile.publicKey },
                { "dateIssued", profile.dateIssued.ToString() },
                { "accountType", profile.accountType.ToString() },
                { "accountPrivilege", profile.accountPrivilege.ToString() },
                { "externalID", profile.externalID },
                { "status", profile.status.ToString() },
                { "loginComplete", profile.loginComplete.ToString() },
                { "pwca", profile.pwca.ToString() },
                { "tfaa", profile.tfaa.ToString() }
        };
            return _idenity.getJWTIdentity(tokenObj);

        }

        private async Task<bool> sendPasswordResetEmail(string username, string password, string email) {
            string content = File.ReadAllText(@"MailTemplates\generalTemplate.html");
            string body = $"<h3>Your password has been reset. Use the following information to login</h3><blockquote align='center' style='font-size:16px; color:maroon;'>Username: {username} <br/> Password: {password} <br/> <blockquote><h4>Request Details</h4> UserAgent : {_idenity.useragent} <br/> IP: {_idenity.IPAddress} </blockquote></blockquote><strong>This is an interim credential. Please login and change your password immediately</strong><br/><i>Please do not share this email with a third party. If you did not request a password change, login and change your password, no further action is required.</i>";
            content = content.Replace("{{RequestBody}}", body);
            content = content.Replace("{{RequestType}}", "Password Reset");
            MailEnvelope mailParam = new MailEnvelope() { subject = $"Password Change Request", body = content, toAddress = new string[] { email }, toName = new string[] { username } };
            return await _mail.send(mailParam);
        }
    }
}
