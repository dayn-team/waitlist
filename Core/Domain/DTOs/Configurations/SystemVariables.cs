namespace Core.Domain.DTOs.Configurations {
    public class SystemVariables {
        public string version { get; set; }
        public string appName { get; set; }
        public string environmentName { get; set; }
        public string siteRoot { get; set; }
        public bool debug { get; set; }
        public double Vat { get; set; }
        public double SystemFee { get; set; }
        public bool KeyExpires { get; set; }
        public HashSet<string> adminMails { get; set; }
        public DBConfig MySQL { get; set; }
        public DBConfig SQLite { get; set; }
        public DBConfig MongoDB { get; set; }
        public ElasticSearch ElasticSearch { get; set; }
        public KeySalt KeySalt { get; set; }
        public JWTSettings JWTSettings { get; set; }
        public QueueServer QueueServer { get; set; }
        public TermiiConfig Termii { get; set; }
        public EmailParam EmailParam { get; set; }
        public AzureStorage AzureStorage { get; set; }
        public GoogleCloudStorageConfig GoogleCloudStorageConfig {
            get; set;
        }
        public TOTP TOPTConfig { get; set; }
        public Stripe StripeConfig { get; set; }
        public PaystackConfiguration PaystackConfiguration { get; set; }
        public SudoConfig SudoConfig { get; set; }
    }
    public class SudoConfig {
        public string token { get; set; }
        public string debitAccountReference { get; set; }
        public string poolAccountName { get; set; }
        public string poolAccountNumber { get; set; }
        public string poolAccountBank { get; set; }
        public string currency { get; set; }
        public string issuerCountry { get; set; }
        public string rootURL { get; set; }
        public string companyRCN { get; set; }
        public SudoEndpoint Endpoints { get; set; }
    }
    public class SudoEndpoint {
        public string fundtransfer { get; set; }
        public string createCard { get; set; }
        public string createCustomer { get; set; }
        public string getCardBalance { get; set; }
        public string getCardTransactionHist { get; set; }
        public string getCardByCustomer { get; set; }
        public string CardUpdate { get; set; }
    }
    public class PaystackConfiguration {
        public string apiKey { get; set; }
        public string rootURL { get; set; }
        public PaystackEndpoint endpoint { get; set; }
    }

    public class PaystackEndpoint {
        public string authInitialize { get; set; }
        public string verifyAuthorization { get; set; }
        public string verifyTransaction { get; set; }
        public string chargeAuthorization { get; set; }
        public string deactivateAuthorization { get; set; }
    }
    public class Stripe {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string WebhookID { get; set; }
        public string WebhookSecret { get; set; }
    }
    public class TOTP {
        public string Secret { get; set; }
        public string APIKey { get; set; }
        public string RootURL { get; set; }
        public string Enroll { get; set; }
        public string Update { get; set; }
        public string RequestVerification { get; set; }
        public string ConfirmVerification { get; set; }
    }
    public class GoogleMapAPI {
        public string distanceURL { get; set; }
        public string APIKey { get; set; }
    }

    public class EmailParam {
        public string fromAddress { get; set; }
        public string fromName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string smtpServer { get; set; }
        public int smtpPort { get; set; }
    }
    
    public class TermiiConfig {
        public string root { get; set; }
        public string sendSMS { get; set; }
        public string sendOTP { get; set; }
        public string verify { get; set; }
        public string api_key { get; set; }
        public string senderID { get; set; }
    }

    public class QueueServer {
        public string mqhost { get; set; }
        public string mquser { get; set; }
        public string mqpw { get; set; }
        public Jobs Jobs { get; set; }
    }
    public class Jobs {
        public string Errlog { get; set; }
    }
    public class JWTSettings {
        public string jwtSecret { get; set; }
        public bool identityExpires { get; set; } = true;
        public int identityExpiryMins { get; set; } = 1440; //1 Day
    }
    public class DBConfig {
        public string server { get; set; }
        public string port { get; set; }
        public string database { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string protocol { get; set; } = "None";
        public string sslMode { get; set; } = "None";
        public string authMechanism { get; set; }
    }
    public class GoogleCloudStorageConfig {
        public string credentialFile { get; set; }
        public string bucketName { get; set; }
    }
    public class ElasticSearch {
        public BasicAuthentication BasicAuthentication { get; set; }
        public string[] nodes { get; set; }
        public ApiKeyAuthentication ApiKeyAuthentication { get; set; }
    }
    public class BasicAuthentication {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class ApiKeyAuthentication {
        public string id { get; set; }
        public string apiKey { get; set; }
    }
    public class KeySalt {
        public string salt { get; set; }
        public int saltIndex { get; set; }
    }
    public class AzureStorage {
        public string connectionString { get; set; }
        public string accountName { get; set; }
        public string accountKey { get; set; }
        public string containerName { get; set; }
        public int SASExpiryMins { get; set; }
    }
}
