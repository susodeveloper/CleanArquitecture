using CleanArchitecture.Application.Users.LoginUser;
using CleanArchitecture.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
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

}