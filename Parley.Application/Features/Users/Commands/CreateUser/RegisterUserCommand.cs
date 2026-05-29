using Parley.Application._Shared.DTOs;
using Parley.Application._Shared.Interfaces;
using Parley.Application.Contracts.Interfaces.Security;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Application.Features.Users.Commands.CreateUser;

public record RegisterUserCommand(string Username, string Email, string Password) : IBaseCommand;

public class RegisterUserCommandHandler : IBaseCommandHandler<RegisterUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<BaseResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.AnyAsync(x => x.Email == request.Email, cancellationToken))
        {
            return BaseResponse.Failure("این ایمیل قبلاً ثبت شده است.");
        }

        if (await _userRepository.AnyAsync(x => x.Username == request.Username, cancellationToken))
        {
            return BaseResponse.Failure("این نام کاربری قبلاً انتخاب شده است.");
        }

        var hashedPass = _passwordHasher.Hash(request.Password);
        var user = new User(request.Username, hashedPass, request.Email);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("ثبت‌نام با موفقیت انجام شد.");
    }
}