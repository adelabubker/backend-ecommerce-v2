using System.Security.Claims;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IAuthenticationService authenticationService,
        ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Registration attempt received.");

        var result = await _authenticationService.RegisterAsync(
            request,
            cancellationToken);

        _logger.LogInformation(
            "Registration completed successfully.");

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Login attempt received.");

        var result = await _authenticationService.LoginAsync(
            request,
            cancellationToken);

        _logger.LogInformation(
            "Login completed successfully.");

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogWarning(
                "Authenticated request does not contain a valid user identifier.");

            return Unauthorized();
        }

        _logger.LogInformation(
            "Getting current user profile for user {UserId}.",
            userId);

        var result = await _authenticationService.GetCurrentUserAsync(
            userId,
            cancellationToken);

        return Ok(result);
    }
}

