using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.UserAgg.Entities;

public class User:AggregateRoot<Guid>
{
    public User(string username, string password, string email, string? firstName = null, string? lastName = null)
    {
        Username = username;
        Password = password;
        FirstName = firstName ?? string.Empty;
        LastName = lastName ?? string.Empty;
        Email = email;
    }
    public string Username { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string FullName => string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName) 
        ? Username 
        : $"{FirstName} {LastName}".Trim();
}