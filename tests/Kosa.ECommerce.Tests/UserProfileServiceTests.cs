using Kosa.ECommerce.Application.DTOs.Users;
using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class UserProfileServiceTests
{
    [Fact]
    public async Task GetProfile_ReturnsUser()
    {
        var users = new Mock<IUserRepository>();
        var addresses = new Mock<IAddressRepository>();
        users.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { UserId = 1, FullName = "Kosa User", Email = "user@kosa.com", IsActive = true });

        var service = new UserProfileService(users.Object, addresses.Object, TestHelpers.CreateMapper());
        var result = await service.GetProfileAsync(1);

        Assert.True(result.Success);
        Assert.Equal("Kosa User", result.Data!.FullName);
        Assert.Equal("user@kosa.com", result.Data.Email);
    }

    [Fact]
    public async Task GetProfile_MissingUser_ThrowsNotFound()
    {
        var users = new Mock<IUserRepository>();
        var addresses = new Mock<IAddressRepository>();
        users.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var service = new UserProfileService(users.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetProfileAsync(99));
    }

    [Fact]
    public async Task UpdateProfile_DuplicateEmail_ThrowsConflict()
    {
        var users = new Mock<IUserRepository>();
        var addresses = new Mock<IAddressRepository>();
        users.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { UserId = 1, FullName = "Kosa User", IsActive = true });
        users.Setup(r => r.GetByEmailOrPhoneExcludingUserAsync("taken@kosa.com", 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { UserId = 2 });

        var service = new UserProfileService(users.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<ConflictException>(() => service.UpdateProfileAsync(1, new UpdateUserProfileDto
        {
            FullName = "Kosa User",
            Email = "taken@kosa.com"
        }));
    }

    [Fact]
    public async Task SaveAddress_NewAddress_UsesRouteUserId_NotClientUserId()
    {
        var users = new Mock<IUserRepository>();
        var addresses = new Mock<IAddressRepository>();
        var request = new UserAddressDto
        {
            AddressId = 0,
            UserId = 999, // client tries to inject another user's id
            AddressType = "Home",
            City = "Riyadh",
            Country = "Saudi Arabia"
        };

        addresses.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        addresses.Setup(r => r.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserAddress>());

        var service = new UserProfileService(users.Object, addresses.Object, TestHelpers.CreateMapper());
        var result = await service.SaveAddressAsync(5, request);

        Assert.True(result.Success);
        addresses.Verify(r => r.AddAsync(It.Is<UserAddress>(a => a.UserId == 5), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAddress_NotOwned_ThrowsUnauthorized()
    {
        var users = new Mock<IUserRepository>();
        var addresses = new Mock<IAddressRepository>();
        addresses.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserAddress { AddressId = 3, UserId = 999 });

        var service = new UserProfileService(users.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.DeleteAddressAsync(3, 5));
    }

    [Fact]
    public async Task DeleteAddress_Missing_ThrowsNotFound()
    {
        var users = new Mock<IUserRepository>();
        var addresses = new Mock<IAddressRepository>();
        addresses.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserAddress?)null);

        var service = new UserProfileService(users.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAddressAsync(3, 5));
    }
}
