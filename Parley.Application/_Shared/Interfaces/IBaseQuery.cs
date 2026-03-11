using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application._Shared.Interfaces;

public interface IBaseQuery<TResponse> : IRequest<BaseResponse<TResponse>>
{
}

