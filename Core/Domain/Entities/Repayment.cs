using Core.Domain.DTOs.Requests;
using Core.Domain.ValueObjects;
using Core.Shared;

namespace Core.Domain.Entities {
    public class Repayment : BaseEntity{
        public string transaction { get; protected set; }
        public Participant payer { get; protected set; }
        public EvidenceFile paymentEvidence { get; protected set; }
        public double amountPaid { get; protected set; }
        public long transDate { get; protected set; }
        public List<Message> disputeLog { get; protected set; }
        public bool verified { get; protected set; }
        public Repayment() { }
        public Repayment(RepaymentDTO repayment, Transaction trx, User user) {
            this.transaction = trx.id;
            this.payer = trx.owner;
            this.amountPaid = repayment.amountPaid;
            this.transDate = Utilities.getTodayDate().unixTimestamp;
            this.disputeLog = new List<Message>();
            this.verified = user.phone == trx.owner.phone? false : true;
            this.paymentEvidence = repayment.paymentEvidence;
        }

        public void addComment(Message message, string? toID = null) {
            if (string.IsNullOrEmpty(toID))
                this.disputeLog.Add(message);
            var data = disputeLog.Find(F => F.messageID == toID);
            if (data is null)
                return;
            data.responses.Add(message);
        }
    }
}
