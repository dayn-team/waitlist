using Core.Domain.Enums;

namespace Core.Domain.DTOs.Requests
{
    public class TransactionDTO
    {
        public string lenderId { get; set; }
        public string borrowerId { get; set; }
        public int amount { get; set; }
        public long loanDate { get; set; }
        public RepaymentType repaymentType { get; set; }
        public int installmentAmount { get; set; }
        public int repaymentDuration { get; set; }
        public int numberOfInstallments { get; set; }
    }
}
