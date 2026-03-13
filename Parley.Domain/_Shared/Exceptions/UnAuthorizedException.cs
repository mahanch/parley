namespace Parley.Domain._Shared.Exceptions;

public class UnAuthorizedException:DomainException
{
    public UnAuthorizedException(string message) : base(message)
    {
    }
}