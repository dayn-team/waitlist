
using Core.Domain.Enums;
using Core.Shared;

namespace Core.Domain.Entities;
public class User : BaseEntity
{
    public User(string firstName, string lastName)
    {
        id = Utilities.randomUUID();
        this.firstName = firstName;
        this.lastName = lastName;
        this.createdOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    public string firstName { get; set; }
    public string lastName { get; set; }

    public Gender gender { get; set; }

    public string? password { get; set; }

    public AccountStatus status { get; set; }

    public long createdOn { get; set; }
}