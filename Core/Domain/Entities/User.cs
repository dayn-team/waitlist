
using Core.Domain.DTOs.Requests;
using Core.Domain.Enums;
using Core.Shared;

namespace Core.Domain.Entities;
public class User : BaseEntity
{
    public User(UserSignupDTO dto)
    {
        firstName = dto.firstName;
        lastName = dto.lastName;
        email = dto.email;
        gender = dto.gender;
        password = new Cryptography.AES().encrypt(dto.password);
        createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public Gender gender { get; set; }
    public string? password { get; set; }
    public AccountStatus status { get; set; }
    public long createdOn { get; set; }
}