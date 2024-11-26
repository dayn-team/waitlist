using Core.Application.Errors;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Domain.Attributes;
using Core.Domain.DTOs.Others;
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;
using Core.Shared;

namespace Core.Domain.Entities;
public class User : BaseEntity {
    public string firstName { get; protected set; }
    public string lastName { get; protected set; }
    public string email { get; protected set; }
    public Gender gender { get; protected set; }
    public string? password { get; protected set; }
    public AccountStatus status { get; protected set; }
    public long createdOn { get; protected set; }
    [DBIndex(IndexAttributes.UNIQUE)]
    public string phone { get; protected set; }
    [DBIndex(IndexAttributes.UNIQUE)]
    public string username { get; protected set; }
    public Privilege privilege { get; protected set; }
    public AccountType type { get; protected set; }
    public bool mailVerified { get; protected set; }
    public int tfa { get; protected set; }
    public int passwordChanged { get; protected set; }
    public string publicKey { get; protected set; }
    public string useragent { get; protected set; }
    public string ip { get; protected set; }
    public long lastLogin { get; protected set; }
    public User(UserSignupDTO dto, Func<string, string> passwordManager) {
        cannotBeNullOrEmpty(dto.firstName, dto.phone, dto.lastName, dto.username);
        if (dto.password.Length < 6)
            throw new InputError("The password is invalid.");
        firstName = dto.firstName;
        lastName = dto.lastName;
        email = dto.email;
        gender = dto.gender;
        this.status = AccountStatus.ACTIVE;
        this.phone = dto.phone;
        this.username = dto.username ?? dto.phone;
        password = passwordManager(dto.password);
        createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.privilege = dto.privilege;
        this.type = dto.type;
        this.passwordChanged = 1;
    }
    public Participant getProfileSummary() {
        return new Participant {
            fullname = $"{this.firstName} {this.lastName}",
            gender = this.gender,
            isConsent = false
        };
    }
    public IdentityData login(IIdentityManager identity) {
        this.useragent = identity.useragent;
        this.publicKey = Cryptography.CharGenerator.genID();
        this.ip = identity.IPAddress;
        this.lastLogin = Utilities.getTodayDate().unixTimestamp;
        var identityData = new IdentityData() {
            username = this.username,
            fullname = $"{this.firstName} {this.lastName}",
            publicKey = this.publicKey,
            dateIssued = this.lastLogin,
            accountType = (int)this.type,
            accountPrivilege = (int)this.privilege,
            externalID = this.id,
            status = (int)this.status,
            tfaen = this.tfa
        };
        return identityData;
    }
}