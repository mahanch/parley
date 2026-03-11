using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application._Shared.Interfaces;

public interface IBaseCommandHandler<in TCommand> : IRequestHandler<TCommand, BaseResponse>
    where TCommand : IBaseCommand
{
}
public interface IBaseCommandHandler<in TCommand, TResponseData> : IRequestHandler<TCommand, BaseResponse<TResponseData>>
    where TCommand : IBaseCommand<TResponseData>
{
}