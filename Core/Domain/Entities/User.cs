using Core.Application.Errors;
using Core.Application.Interfaces.Infrastructure.Identity;
using Core.Domain.Attributes;
using Core.Domain.DTOs.Others;
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
using Core.Domain.ValueObjects;
using Core.Shared;
using System.Diagnostics.Metrics;

namespace Core.Domain.Entities;
public class User : BaseEntity {
    public string FullName { get; protected set; }
    public string Email { get; protected set; }
    public Gender Gender { get; protected set; }
    public string? Password { get; protected set; }
    public AccountStatus Status { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    [DBIndex(IndexAttributes.UNIQUE)]
    public string Phone { get; protected set; }
    [DBIndex(IndexAttributes.UNIQUE)]
    public string Username { get; protected set; }
    public Privilege Privilege { get; protected set; }
    public AccountType Type { get; protected set; }
    public bool MailVerified { get; protected set; }
    public int Tfa { get; protected set; }
    public int PasswordChanged { get; protected set; }
    public string PublicKey { get; protected set; }
    public string Useragent { get; protected set; }
    public string Ip { get; protected set; }
    public DateTime LastLogin { get; protected set; }
    public Country Country { get; protected set; }
    public User(UserSignupDTO dto, Func<string, string> passwordManager) {
        cannotBeNullOrEmpty(dto.FullName, dto.Phone, dto.Username);
        if (dto.Password.Length < 6)
            throw new InputError("The password is invalid. Minimum of 6 characters is required");
        this.FullName = dto.FullName;
        this.Email = dto.Email;
        this.Gender = dto.Gender;
        this.Status = AccountStatus.ACTIVE;
        this.Phone = dto.Phone;
        this.Username = dto.Username ?? dto.Phone;
        this.Password = passwordManager(dto.Password);
        this.CreatedOn = DateTime.Now;
        this.Privilege = dto.Privilege;
        this.Type = dto.Type;
        this.PasswordChanged = 1;
        this.Country = dto.Country;
    }
    public Participant getProfileSummary() {
        return new Participant {
            Fullname = this.FullName,
            Gender = this.Gender,
            IsConsent = false
        };
    }
    public IdentityData login(IIdentityManager identity) {
        this.Useragent = identity.Useragent;
        this.PublicKey = Cryptography.CharGenerator.genID();
        this.Ip = identity.IPAddress;
        this.LastLogin = DateTime.Now;
        var identityData = new IdentityData() {
            Username = this.Username,
            Fullname = this.FullName,
            PublicKey = this.PublicKey,
            DateIssued = this.LastLogin,
            AccountType = (int)this.Type,
            AccountPrivilege = (int)this.Privilege,
            ExternalID = this.Id,
            Status = (int)this.Status,
            Tfaen = this.Tfa
        };
        return identityData;
    }
}