using Asp.Versioning;
using CleanArchitecture.Api.Utils;
using CleanArchitecture.Application.Users.GetUsersDapperPagination;
using CleanArchitecture.Application.Users.GetUsersPagination;
using CleanArchitecture.Application.Users.LoginUser;
using CleanArchitecture.Application.Users.RegisterUser;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Users;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [MapToApiVersion(ApiVersions.V1)]
    public async Task<IActionResult> LoginV1(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new LoginCommand(request.Email, request.Password);
        var resultados = await _sender.Send(command, cancellationToken);
        
        return resultados.IsFailure ? Unauthorized(resultados.Error) : Ok(resultados.Value);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Nombre,
            request.Apellidos,
            request.Password
        );

        var resultados = await _sender.Send(command, cancellationToken);
        
        return resultados.IsFailure ? Unauthorized(resultados.Error) : Ok(resultados.Value);
    }

    [AllowAnonymous]
    [HttpGet("getPagination", Name = "PaginationUsers")]
    [ProducesResponseType(typeof(PagedResults<User, UserId>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResults<User, UserId>>> GetPagination(
        [FromQuery] GetUsersPaginationQuery request
    )
    {
        var resultados = await _sender.Send(request);
        
        return resultados.IsFailure ? NotFound(resultados.Error) : Ok(resultados.Value);
    }

    [AllowAnonymous]
    [HttpGet("getPaginationDapper", Name = "GetPaginationDapper")]
    [ProducesResponseType(typeof(PagedDapperResults<UserPaginationData>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedDapperResults<UserPaginationData>>> GetPaginationDapper(
        [FromQuery] GetUsersDapperPaginationQuery request
    )
    {
        var resultados = await _sender.Send(request);
        
        return resultados.IsFailure ? NotFound(resultados.Error) : Ok(resultados.Value);
    }




}