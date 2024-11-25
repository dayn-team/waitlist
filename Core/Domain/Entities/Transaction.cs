using Core.Application.Errors;
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;
using Core.Shared;

namespace Core.Domain.Entities;
public class Transaction : BaseEntity {
    public long dateCreated { get; protected set; }
    public long loanDate { get; protected set; }
    public double amount { get; protected set; }
    public long paybackDate { get; protected set; }
    public Participant owner { get; protected set; }
    public List<Participant> witnesses { get; protected set; }
    public Participant creditor { get; protected set; }
    public CreatorClass createdBy { get; protected set; }
    public LoanStatus status { get; protected set; }
    public PaymentPlan paymentPlan { get; protected set; }
    public Transaction() { }
    public Transaction(TransactionDTO dto, User user) {
        if (dto.amount < 1)
            throw new InputError("Invalid Amount");
        if (dto.paybackDate < Utilities.getTodayDate().unixTimestamp)
            throw new InputError("Invalid Pay back date. Must be in the future");
        if (user is null)
            throw new AuthenticationError("Invalid Account");
        if (dto.createdBy is null)
            throw new InputError("Invalid input value for Who is creating the entry");
        if(dto.otherParty is null) {
            if(dto.createdBy == CreatorClass.CREDITOR) {
                throw new InputError("You must specify the account details of the Loanee");
            } else {
                throw new InputError("You must specify the account details of the creditor");
            }
        }
        if (string.IsNullOrEmpty(dto.otherParty.phone))
            throw new InputError("Phone number of other party is required.");

        if (dto.paymentPlan is null)
            throw new InputError("Payment plan is a required field");

        validateWitness(dto.witnesses);

        this.amount = dto.amount;
        this.createdBy = (CreatorClass)dto.createdBy;
        this.creditor = this.createdBy == CreatorClass.CREDITOR ? user.getProfileSummary() : dto.otherParty;
        this.dateCreated = Utilities.getTodayDate().unixTimestamp;
        this.loanDate = dto.loanDate;
        this.owner = this.createdBy == CreatorClass.LOANEE ? user.getProfileSummary() : dto.otherParty;
        this.paybackDate = dto.paybackDate;
        this.paymentPlan = dto.paymentPlan;
        this.status = LoanStatus.PENDING;
        this.witnesses = dto.witnesses;
    }

    private void validateWitness(List<Participant> witnesses) {
        if (witnesses is null)
            throw new InputError("Invalid witness list");
        if(witnesses.Count < 4)
            throw new InputError("You need at least 4 witnesses to complete this action");
        var maleCount = witnesses.Count(F=> F.gender == Gender.MALE);
        var femaleCount = witnesses.Count(F=> F.gender == Gender.FEMALE);
        var left = 4 - maleCount;
        if(left > 0) {
            if (left * 2 > femaleCount)
                throw new InputError($"You need additional {left * 2 - femaleCount} female witnesse(s)");
        }
    }
}