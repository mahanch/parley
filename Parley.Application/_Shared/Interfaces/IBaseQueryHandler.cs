using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application._Shared.Interfaces;

public interface IBaseQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, BaseResponse<TResponse>>
    where TQuery : IBaseQuery<TResponse>
{
}

