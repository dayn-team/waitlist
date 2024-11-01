
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;

using Core.Shared;


namespace Core.Domain.Entities;
public class Transaction : BaseEntity
{
    public Transaction(TransactionDTO dto)
    {
        lenderId = dto.lenderId;
        borrowerId = dto.borrowerId;
        amount = dto.amount;
        loanDate = dto.loanDate;
        repaymentType = dto.repaymentType;
        installmentAmount = dto.installmentAmount;
        repaymentDuration = dto.repaymentDuration;
        numberOfInstallments = dto.numberOfInstallments;
        createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
    }

    public string lenderId { get; set; }
    public User? lender { get; set; }
    public string borrowerId { get; set; }
    public User? borrower { get; set; }
    public int amount { get; set; }
    public long loanDate { get; set; }
    public RepaymentType repaymentType { get; set; }
    public int installmentAmount { get; set; } // will be the same as loan amount if repayment type is one off
    public int repaymentDuration { get; set; } // in years
    public int numberOfInstallments { get; set; } // 1 if repayment type is one off
    public long createdOn { get; set; }

}