using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Authentication;
using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IJwtTokenGenerator> _tokenGenerator = new();

    private AuthenticationService CreateService()
    {
        _tokenGenerator.Setup(t => t.CreateToken(It.IsAny<User>())).Returns("test-token");
        return new AuthenticationService(_userRepository.Object, _passwordHasher.Object, _tokenGenerator.Object, TestHelpers.CreateMapper());
    }

    [Fact]
    public async Task Register_Success_HashesPasswordAndReturnsToken()
    {
        _userRepository.Setup(r => r.GetByEmailOrPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _userRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _passwordHasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed-value");

        var service = CreateService();
        var result = await service.RegisterAsync(new RegisterRequestDto
        {
            FullName = "Test User",
            EmailOrPhone = "test@example.com",
            Password = "secret123"
        });

        Assert.True(result.Success);
        Assert.Equal("test-token", result.Data!.Token);
        _passwordHasher.Verify(h => h.Hash("secret123"), Times.Once);
        _userRepository.Verify(r => r.AddAsync(It.Is<User>(u => u.PasswordHash == "hashed-value"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Register_DuplicateAccount_ThrowsConflict()
    {
        _userRepository.Setup(r => r.GetByEmailOrPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { UserId = 1, FullName = "Existing" });

        var service = CreateService();

        await Assert.ThrowsAsync<ConflictException>(() => service.RegisterAsync(new RegisterRequestDto
        {
            FullName = "Test User",
            EmailOrPhone = "test@example.com",
            Password = "secret123"
        }));
    }

    [Fact]
    public async Task Register_EitherEmailOrPhoneIsStored()
    {
        _userRepository.Setup(r => r.GetByEmailOrPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _userRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _passwordHasher.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed-value");

        var service = CreateService();

        var phoneResult = await service.RegisterAsync(new RegisterRequestDto
        {
            FullName = "Mobile User",
            EmailOrPhone = "+966500000000",
            Password = "secret123"
        });

        Assert.True(phoneResult.Success);
        _userRepository.Verify(r => r.AddAsync(It.Is<User>(u => u.Phone == "+966500000000" && u.Email == null), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ThrowsUnauthorized()
    {
        _userRepository.Setup(r => r.GetByEmailOrPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAsync(new LoginRequestDto
        {
            EmailOrPhone = "nobody@example.com",
            Password = "wrong"
        }));
    }

    [Fact]
    public async Task Login_WrongPassword_ThrowsUnauthorized()
    {
        var user = new User { UserId = 1, FullName = "Test", PasswordHash = "stored-hash" };
        _userRepository.Setup(r => r.GetByEmailOrPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(h => h.Verify("wrong", "stored-hash")).Returns(false);

        var service = CreateService();

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.LoginAsync(new LoginRequestDto
        {
            EmailOrPhone = "test@example.com",
            Password = "wrong"
        }));
    }

    [Fact]
    public async Task GetCurrentUser_MissingUser_ThrowsNotFound()
    {
        _userRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetCurrentUserAsync(999));
    }
}
