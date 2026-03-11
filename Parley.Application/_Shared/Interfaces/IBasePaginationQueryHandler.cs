using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application._Shared.Interfaces;

// public interface IBasePaginationQueryHandler<in TQuery, TResponse> 
//     : IRequestHandler<TQuery, BaseResponse<PaginationResult<TResponse>>>
//     where TQuery : PaginationRequest, IBasePaginationQuery<TResponse>
//     where TResponse : class
// {
//     
// }