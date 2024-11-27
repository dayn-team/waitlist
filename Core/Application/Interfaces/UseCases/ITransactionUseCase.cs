using Core.Domain.DTOs.Filter;
using Core.Domain.DTOs.Requests;
using Core.Domain.DTOs.Response;

namespace Core.Application.Interfaces.UseCases {
    public interface ITransactionUseCase {
        Task<WebResponse<object>> createTransactionEntry(TransactionDTO transaction);
        Task<WebResponse<object>> getTransaction(TransactionFilter filter);
        Task<WebResponse<object>> getPaymentLog(TransactionFilter filter);
        Task<WebResponse<object>> createPaymentLog(RepaymentDTO repayment);
        Task<WebResponse<object>> addMessage(DisputeRequest request);

        Task<WebResponse<object>> addConsent(string transactionID);
    }
}
