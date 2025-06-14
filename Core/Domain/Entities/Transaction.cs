using Core.Application.Errors;
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;
using Core.Shared;

namespace Core.Domain.Entities;
public class Transaction : BaseEntity {
    public DateTime DateCreated { get; protected set; }
    public DateTime LoanDate { get; protected set; }
    public double Amount { get; protected set; }
    public DateTime PaybackDate { get; protected set; }
    public Participant Owner { get; protected set; }
    public List<Participant> Witnesses { get; protected set; }
    public Participant Creditor { get; protected set; }
    public CreatorClass CreatedBy { get; protected set; }
    public LoanStatus Status { get; protected set; }
    public PaymentPlan PaymentPlan { get; protected set; }
    public Transaction() { }
    public Transaction(TransactionDTO dto, User user) {
        if (dto.Amount < 1)
            throw new InputError("Invalid Amount");
        if (dto.PaybackDate < Utilities.GetTodayDate().unixTimestamp)
            throw new InputError("Invalid Pay back date. Must be in the future");
        if (user is null)
            throw new AuthenticationError("Invalid Account");
        if (dto.CreatedBy is null)
            throw new InputError("Invalid input value for Who is creating the entry");
        if(dto.OtherParty is null) {
            if(dto.CreatedBy == CreatorClass.CREDITOR) {
                throw new InputError("You must specify the account details of the Loanee");
            } else {
                throw new InputError("You must specify the account details of the creditor");
            }
        }
        if (string.IsNullOrEmpty(dto.OtherParty.Phone))
            throw new InputError("Phone number of other party is required.");

        if (dto.PaymentPlan is null)
            throw new InputError("Payment plan is a required field");

        ValidateWitness(dto.Witnesses);

        this.Amount = dto.Amount;
        this.CreatedBy = (CreatorClass)dto.CreatedBy;
        this.Creditor = this.CreatedBy == CreatorClass.CREDITOR ? user.getProfileSummary() : dto.OtherParty;
        this.DateCreated = DateTime.Now;
        this.LoanDate = dto.LoanDate;
        this.Owner = this.CreatedBy == CreatorClass.LOANEE ? user.getProfileSummary() : dto.OtherParty;
        this.PaybackDate = dto.PaybackDate;
        this.PaymentPlan = dto.PaymentPlan;
        this.Status = LoanStatus.PENDING;
        this.Witnesses = dto.Witnesses;
    }

    private void ValidateWitness(List<Participant> witnesses) {
        if (witnesses is null)
            throw new InputError("Invalid witness list");
        if(witnesses.Count < 4)
            throw new InputError("You need at least 4 witnesses to complete this action");
        var maleCount = witnesses.Count(F=> F.Gender == Gender.MALE);
        var femaleCount = witnesses.Count(F=> F.Gender == Gender.FEMALE);
        var left = 4 - maleCount;
        if(left > 0) {
            if (left * 2 > femaleCount)
                throw new InputError($"You need additional {left * 2 - femaleCount} female witness(es)");
        }
        if (witnesses.GroupBy(F => F.Phone).Any(F => F.Count() > 1))
            throw new InputError("Some repetition(s) exists in the witness list.");
    }

    public void AddConsent(string personID) {
        var witness = Witnesses.Find(F => F.Username == personID);
        if (witness is null)
            throw new InputError("Account not found in witness list");
        witness.IsConsent = true;
    }
}