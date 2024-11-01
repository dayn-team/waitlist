
using Core.Domain.Enums;

using Core.Shared;


namespace Core.Domain.Entities;
public class Transaction : BaseEntity
{
    public Transaction(string lenderId, string borrowerId, RepaymentType repaymentType)
    {
        id = Utilities.randomUUID();
        this.lenderId = lenderId;
        this.borrowerId = borrowerId;
        this.borrowerId = borrowerId;
        this.repaymentType = repaymentType;
        this.createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
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