using Core.Application.Errors;
using Core.Application.Interfaces.Email;
using Core.Application.Interfaces.Infrastructure.Cache;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Application.Interfaces.Infrastructure.Repository;
using Core.Application.Interfaces.UseCases;
using Core.Domain.DTOs.Configurations;
using Core.Domain.DTOs.Filter;
using Core.Domain.DTOs.Requests;
using Core.Domain.DTOs.Response;
using Core.Shared;
using Microsoft.Extensions.Options;

namespace Core.Application.UseCases {
    public class TransactionUseCase : BaseUseCase, ITransactionUseCase {
        private readonly IUserRepository _account;
        private readonly SystemVariables _sysVar;
        private readonly IIdentityManager _identity;
        private readonly IEmailService _mail;
        private readonly PasswordManager _passManager;
        private readonly ICacheService _cache;
        private readonly ITransactionRepository _trxRepo;
        private readonly IRepaymentRepository _repayRepo;
        public TransactionUseCase(IUserRepository account, IOptionsMonitor<SystemVariables> sysVar, IIdentityManager identity, IEmailService email, ICacheService cache, ITransactionRepository trxRepo, IRepaymentRepository repayRepo) : base(sysVar, cache, identity) {
            _account = account;
            _sysVar = sysVar.CurrentValue;
            _identity = identity;
            _passManager = new PasswordManager(_sysVar.KeySalt);
            _mail = email;
            _cache = cache;
            _trxRepo = trxRepo;
            _repayRepo = repayRepo;
        }

        public async Task<WebResponse<object>> addConsent(string transactionID) {
            await verifySession();
            WebResponse response = new WebResponse();
            var trx = await _trxRepo.get(transactionID);
            if (trx is null)
                throw new NotFoundError("Invalid Transaction");
            trx.addConsent(profile.username);
            await _trxRepo.update(trx);
            return response.success();
        }

        public Task<WebResponse<object>> addMessage(DisputeRequest request) {
            throw new NotImplementedException();
        }

        public async Task<WebResponse<object>> createPaymentLog(RepaymentDTO repayment) {
            await verifySession();
            WebResponse response = new WebResponse();
            throw new Exception();
        }

        public async Task<WebResponse<object>> createTransactionEntry(TransactionDTO transaction) {
            await verifySession();
            WebResponse response = new WebResponse();
            throw new Exception();
        }

        public Task<WebResponse<object>> getPaymentLog(TransactionFilter filter) {
            throw new NotImplementedException();
        }

        public Task<WebResponse<object>> getTransaction(TransactionFilter filter) {
            throw new NotImplementedException();
        }
    }
}
