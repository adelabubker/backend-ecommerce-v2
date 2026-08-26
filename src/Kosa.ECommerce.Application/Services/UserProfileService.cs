using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Users;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class UserProfileService : IUserProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public UserProfileService(IUserRepository userRepository, IAddressRepository addressRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserProfileDto>> GetProfileAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User was not found.");
        }

        return Result<UserProfileDto>.Ok(_mapper.Map<UserProfileDto>(user));
    }

    public async Task<Result<UserProfileDto>> UpdateProfileAsync(int userId, UpdateUserProfileDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User was not found.");
        }

        var normalizedEmail = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim().ToLower();
        var normalizedPhone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim().ToLower();

        if (normalizedEmail is not null)
        {
            var existing = await _userRepository.GetByEmailOrPhoneExcludingUserAsync(normalizedEmail, userId, cancellationToken);
            if (existing is not null)
            {
                throw new ConflictException("Email or phone already in use.");
            }
        }

        if (normalizedPhone is not null)
        {
            var existing = await _userRepository.GetByEmailOrPhoneExcludingUserAsync(normalizedPhone, userId, cancellationToken);
            if (existing is not null)
            {
                throw new ConflictException("Email or phone already in use.");
            }
        }

        user.FullName = request.FullName.Trim();
        user.Email = normalizedEmail;
        user.Phone = normalizedPhone;
        user.ProfileImage = request.ProfileImage;

        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result<UserProfileDto>.Ok(_mapper.Map<UserProfileDto>(user), "Profile updated successfully.");
    }

    public async Task<Result<IReadOnlyList<UserAddressDto>>> GetAddressesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var addresses = await _addressRepository.GetByUserIdAsync(userId, cancellationToken);
        return Result<IReadOnlyList<UserAddressDto>>.Ok(_mapper.Map<IReadOnlyList<UserAddressDto>>(addresses));
    }

    public async Task<Result<int>> SaveAddressAsync(int userId, UserAddressDto request, CancellationToken cancellationToken = default)
    {
        UserAddress address;

        if (request.AddressId == 0)
        {
            address = _mapper.Map<UserAddress>(request);
            address.UserId = userId;
            address.AddressId = 0; // never trust a client-supplied id on create
            await _addressRepository.AddAsync(address, cancellationToken);
        }
        else
        {
            var existing = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken)
                ?? throw new NotFoundException("Address was not found.");

            if (existing.UserId != userId)
            {
                throw new UnauthorizedException("You cannot modify another user's address.");
            }

            _mapper.Map(request, existing);
            existing.UserId = userId;
            address = existing;
        }

        // Rule 1: the newly saved "default" clears the flag on all other addresses.
        if (address.IsDefault)
        {
            var others = (await _addressRepository.GetByUserIdAsync(userId, cancellationToken)) ?? new List<UserAddress>();
            foreach (var other in others.Where(a => a.AddressId != address.AddressId && a.IsDefault))
            {
                other.IsDefault = false;
                _addressRepository.Update(other);
            }
        }

        await _addressRepository.SaveChangesAsync(cancellationToken);

        // Rule 2: a user always keeps exactly one default address when they have any.
        var all = (await _addressRepository.GetByUserIdAsync(userId, cancellationToken)) ?? new List<UserAddress>();
        if (!all.Any(a => a.IsDefault))
        {
            address.IsDefault = true;
            await _addressRepository.SaveChangesAsync(cancellationToken);
        }

        return Result<int>.Ok(address.AddressId, "Address saved successfully.");
    }

    public async Task<Result<int>> DeleteAddressAsync(int addressId, int? userId = null, CancellationToken cancellationToken = default)
    {
        var address = await _addressRepository.GetByIdAsync(addressId, cancellationToken);
        if (address is null)
        {
            throw new NotFoundException("Address was not found.");
        }
        if (userId.HasValue && address.UserId != userId.Value)
        {
            throw new UnauthorizedException("You cannot delete another user's address.");
        }

        var ownerId = address.UserId;
        var wasDefault = address.IsDefault;

        _addressRepository.Delete(address);
        await _addressRepository.SaveChangesAsync(cancellationToken);

        // Rule 3: promote the first remaining address when the default is deleted.
        if (wasDefault)
        {
            var remaining = await _addressRepository.GetByUserIdAsync(ownerId, cancellationToken);
            var promoted = remaining.OrderBy(a => a.AddressId).FirstOrDefault();
            if (promoted is not null)
            {
                promoted.IsDefault = true;
                await _addressRepository.SaveChangesAsync(cancellationToken);
            }
        }

        return Result<int>.Ok(addressId, "Address deleted successfully.");
    }
}
