using Parley.Application._Shared.DTOs;
using Parley.Application._Shared.Interfaces;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string FirstName, string LastName, string UserName, string Password,string Email):IBaseCommand;

public class CreateUserCommandHandler:IBaseCommandHandler<CreateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IUserRepository _userRepository;
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<BaseResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.UserName, request.Password, request.FirstName, request.LastName, request.Email);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new BaseResponse();
    }
}