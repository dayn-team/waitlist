using Core.Domain.DTOs.Requests;
using Core.Domain.ValueObjects;
using Core.Shared;

namespace Core.Domain.Entities {
    public class Repayment : BaseEntity{
        public string Transaction { get; protected set; }
        public Participant Payer { get; protected set; }
        public EvidenceFile PaymentEvidence { get; protected set; }
        public double AmountPaid { get; protected set; }
        public long TransDate { get; protected set; }
        public List<Message> DisputeLog { get; protected set; }
        public bool Verified { get; protected set; }
        public Repayment() { }
        public Repayment(RepaymentDTO repayment, Transaction trx, User user) {
            this.Transaction = trx.Id;
            this.Payer = trx.Owner;
            this.AmountPaid = repayment.amountPaid;
            this.TransDate = Utilities.GetTodayDate().unixTimestamp;
            this.DisputeLog = new List<Message>();
            this.Verified = user.Phone == trx.Owner.Phone? false : true;
            this.PaymentEvidence = repayment.paymentEvidence;
        }

        public void AddComment(Message message, string? toID = null) {
            if (string.IsNullOrEmpty(toID))
                this.DisputeLog.Add(message);
            var data = DisputeLog.Find(F => F.MessageID == toID);
            if (data is null)
                return;
            data.Responses.Add(message);
        }
    }
}
