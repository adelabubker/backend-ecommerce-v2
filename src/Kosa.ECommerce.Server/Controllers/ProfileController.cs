using System.Security.Claims;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/profile")]
public sealed class ProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public ProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _userProfileService.GetProfileAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto request, CancellationToken cancellationToken)
    {
        var result = await _userProfileService.UpdateProfileAsync(CurrentUserId, request, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("addresses")]
    public async Task<IActionResult> GetAddresses(CancellationToken cancellationToken)
    {
        var result = await _userProfileService.GetAddressesAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("addresses")]
    public async Task<IActionResult> SaveAddress([FromBody] UserAddressDto request, CancellationToken cancellationToken)
    {
        var result = await _userProfileService.SaveAddressAsync(CurrentUserId, request, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("addresses/{addressId:int}")]
    public async Task<IActionResult> DeleteAddress(int addressId, CancellationToken cancellationToken)
    {
        var result = await _userProfileService.DeleteAddressAsync(addressId, CurrentUserId, cancellationToken);
        return Ok(result);
    }
}
