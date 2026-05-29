namespace Parley.Application.Contracts.Interfaces.Security;

public interface IAuthHelper
{
    string GenerateJwtToken(Guid userId, string username, string email);
    Guid GetUserId();
    string GetUsername();
}
