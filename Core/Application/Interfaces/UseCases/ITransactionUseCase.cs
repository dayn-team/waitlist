using Core.Domain.DTOs.Filter;
using Core.Domain.DTOs.Requests;
using Core.Domain.DTOs.Response;

namespace Core.Application.Interfaces.UseCases {
    public interface ITransactionUseCase {
        Task<WebResponse<object>> CreateTransactionEntry(TransactionDTO transaction);
        Task<WebResponse<object>> GetTransaction(TransactionFilter filter);
        Task<WebResponse<object>> GetPaymentLog(TransactionFilter filter);
        Task<WebResponse<object>> CreatePaymentLog(RepaymentDTO repayment);
        Task<WebResponse<object>> AddMessage(DisputeRequest request);

        Task<WebResponse<object>> AddConsent(string transactionID);
    }
}
