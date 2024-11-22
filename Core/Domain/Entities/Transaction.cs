using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;
using System.Security.Policy;

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
    public EvidenceFile loanEvidence { get; protected set; }
    public EvidenceFile paymentEvidence { get; protected set; }

    public Transaction(TransactionDTO dto) {
        
    }
}