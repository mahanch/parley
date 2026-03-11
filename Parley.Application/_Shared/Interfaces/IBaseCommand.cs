using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application._Shared.Interfaces;

public interface IBaseCommand : IRequest<BaseResponse>
{
}

public interface IBaseCommand<TResponse> : IRequest<BaseResponse<TResponse>>
{
}


