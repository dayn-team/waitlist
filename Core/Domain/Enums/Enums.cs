namespace Core.Domain.Enums
{
    public enum AccountType
    {
        CUSTOMER,
        CLIENT,
        ADMIN
    }
    public enum Gender
    {
        MALE,
        FEMALE
    }
    public enum Privilege
    {
        SUPERADMIN = 200,
        ADMIN = 100,
    }
    public enum AccountStatus
    {
        ACTIVE = 1,
        SUSPENDED = -1,
        DELETED = -2
    }
    public enum LoanStatus
    {
        CANCELLED = -1,
        AWAITING_WITNESSES,
        PENDING,
        PAID,
        FORGIVEN
    }

    public enum ActivityType
    {
        LOGIN,
        ACCOUNT_STATUS
    }

    public enum AuthType
    {
        EMAIL,
        OTP,
        SMS
    }
    public enum TransactionType
    {
        LOAN,
        LOAN_REPAYMENT
    }
    public enum RepaymentType
    {
        ONE_OFF,
        INSTALLMENT
    }
    public enum EvidenceType
    {
        LOAN_DISPATCH,
        LOAN_RECEIPT,
        REPAYMENT_DISPATCH,
        REPAYMENT_RECEIPT,
    }
    public enum TransactionClass
    {
        DEBIT,
        CREDIT,
        UNCLASS
    }
    public enum Events
    {
        TYPING,
        TYPING_STOPPED,
        MESSAGE_READ,
        GET_NEXT_MESSAGE,
        GET_PREVIOUS_MESSAGE,
        NOTIFICATION_UPDATE,
        MESSAGE_DELIVERY,
        CHAT_LIST,
        CREATE_CHAT,
        AUTH,
        GENERIC,
        SET_CHAT_WITH,
        ERROR,
        MESSAGE_READ_UPDATE,
        NEW_MESSAGE,
        TYPING_UPDATE
    }

    public enum CreatorClass {
        CREDITOR,
        LOANEE
    }

    public enum PaymentFrequency {
        DAILY,
        WEEKLY,
        MONTHLY,
        YEARLY
    }
}
