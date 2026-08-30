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
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IUserProfileService userProfileService,
        ILogger<ProfileController> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProfile(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting profile for user {UserId}.",
            CurrentUserId);

        var result = await _userProfileService.GetProfileAsync(
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateUserProfileDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Updating profile for user {UserId}.",
            CurrentUserId);

        var result = await _userProfileService.UpdateProfileAsync(
            CurrentUserId,
            request,
            cancellationToken);

        _logger.LogInformation(
            "Profile updated successfully for user {UserId}.",
            CurrentUserId);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("addresses")]
    public async Task<IActionResult> GetAddresses(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting addresses for user {UserId}.",
            CurrentUserId);

        var result = await _userProfileService.GetAddressesAsync(
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("addresses")]
    public async Task<IActionResult> SaveAddress(
        [FromBody] UserAddressDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Saving address for user {UserId}.",
            CurrentUserId);

        var result = await _userProfileService.SaveAddressAsync(
            CurrentUserId,
            request,
            cancellationToken);

        _logger.LogInformation(
            "Address saved successfully for user {UserId}.",
            CurrentUserId);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("addresses/{addressId:int}")]
    public async Task<IActionResult> DeleteAddress(
        int addressId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Deleting address {AddressId} for user {UserId}.",
            addressId,
            CurrentUserId);

        var result = await _userProfileService.DeleteAddressAsync(
            addressId,
            CurrentUserId,
            cancellationToken);

        _logger.LogInformation(
            "Address {AddressId} deleted successfully for user {UserId}.",
            addressId,
            CurrentUserId);

        return Ok(result);
    }
}