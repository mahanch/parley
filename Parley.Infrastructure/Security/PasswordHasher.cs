using Parley.Application.Contracts.Interfaces.Security;

namespace Parley.Infrastructure.Security;

using Microsoft.AspNetCore.Identity;

public class PasswordHasher:IPasswordHasher
{
    private readonly IPasswordHasher<object> _hasher = new PasswordHasher<object>();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, password);
        return result == PasswordVerificationResult.Success 
               || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}