namespace Core.Domain.Enums {
    public enum AccountType {
        CUSTOMER,
        CLIENT,
        ADMIN
    }
    public enum Privilege {
        SUPERADMIN = 200,
        ADMIN = 100,
    }
    public enum AccountStatus {
        ACTIVE = 1,
        SUSPENDED = -1,
        DELETED = -2
    }
    public enum OrderStatus {
        CANCELLED = -1,
        PENDING,
        PAID,
        ASSIGNED,
        COMPLETED
    }

    public enum ActivityType {
        LOGIN,
        ACCOUNT_STATUS
    }

    public enum AuthType {
        EMAIL,
        OTP,
        SMS
    }
    public enum TransactionType {
        WITHDRAWAL,
        PAYMENT,
        POOL_FUNDING,
        CARD_FUNDING
    }
    public enum TransactionClass {
        DEBIT,
        CREDIT,
        UNCLASS
    }
    public enum Events {
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
}
