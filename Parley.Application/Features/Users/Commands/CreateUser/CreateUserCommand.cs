using Parley.Application._Shared.DTOs;
using Parley.Application._Shared.Interfaces;
using Parley.Application.Contracts.Interfaces.Security;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string FirstName, string LastName, string UserName, string Password,string Email):IBaseCommand;

public class CreateUserCommandHandler:IBaseCommandHandler<CreateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<BaseResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.AnyAsync(x=>x.Email == request.Email, cancellationToken))
        {
            return BaseResponse.Failure("Email already exists");
        }

        if (await  _userRepository.AnyAsync(x=>x.Username == request.UserName, cancellationToken))
        {
            return BaseResponse.Failure("Username already exists.Try to Login");
        }
        
        var pass =_passwordHasher.Hash(request.Password);
        var user = new User(request.UserName, pass, request.FirstName, request.LastName, request.Email);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BaseResponse.Success();
    }
}