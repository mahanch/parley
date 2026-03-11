using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parley.Application._Shared.DTOs;
using Parley.Application.Features.Users.Commands.CreateUser;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> CreateUser([FromBody] CreateUserCommand command)
    {
        var res =await _mediator.Send(command);
        return Ok(res);
    }
}