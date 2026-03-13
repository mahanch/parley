namespace Parley.Domain._Shared.Exceptions;

public class BadRequestException:DomainException
{
    public BadRequestException(string message):base(message)
    {
        
    }

    public BadRequestException(string message, string details) : base(message)
    {
        Details = details;
    }

    public BadRequestException(string message,  Dictionary<string, object?> extra) :
        base(message)
    {
        Extra = extra;
    }


    public string? Details { get; }
    public Dictionary<string, object?>? Extra { get; }
    
}