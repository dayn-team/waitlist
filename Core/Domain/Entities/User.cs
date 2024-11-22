using Core.Application.Errors;
using Core.Domain.Attributes;
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
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
    public User(UserSignupDTO dto) {
        cannotBeNullOrEmpty(dto.firstName, dto.phone, dto.lastName, dto.username);
        if (dto.password.Length < 6)
            throw new InputError("The password is invalid.");
        firstName = dto.firstName;
        lastName = dto.lastName;
        email = dto.email;
        gender = dto.gender;
        this.status = AccountStatus.ACTIVE;
        this.phone = dto.phone;
        this.username = dto.username;
        password = new Cryptography.AES().encrypt(dto.password);
        createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}