using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Authentication;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _mapper = mapper;
    }

    public async Task<Result<AuthUserDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalized = request.EmailOrPhone.Trim().ToLower();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new ValidationException("Email or phone is required.");
        }
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            throw new ValidationException("Password must be at least 6 characters.");
        }

        var existing = await _userRepository.GetByEmailOrPhoneAsync(normalized, cancellationToken);
        if (existing is not null)
        {
            throw new ConflictException("An account with this email or phone already exists.");
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        if (normalized.Contains('@'))
        {
            user.Email = normalized;
        }
        else
        {
            user.Phone = normalized;
        }

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<AuthUserDto>(user);
        result.Token = _tokenGenerator.CreateToken(user);

        return Result<AuthUserDto>.Ok(result, "Registration completed successfully.");
    }

    public async Task<Result<AuthUserDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalized = request.EmailOrPhone.Trim().ToLower();

        var user = await _userRepository.GetByEmailOrPhoneAsync(normalized, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        var result = _mapper.Map<AuthUserDto>(user);
        result.Token = _tokenGenerator.CreateToken(user);

        return Result<AuthUserDto>.Ok(result, "Login successful.");
    }

    public async Task<Result<CurrentUserDto>> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User was not found.");
        }

        return Result<CurrentUserDto>.Ok(_mapper.Map<CurrentUserDto>(user));
    }
}
