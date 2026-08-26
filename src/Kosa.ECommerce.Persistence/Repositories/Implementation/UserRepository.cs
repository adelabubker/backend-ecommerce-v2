using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(KosaDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailOrPhoneAsync(string emailOrPhone, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == emailOrPhone || u.Phone == emailOrPhone, cancellationToken);
    }

    public async Task<User?> GetByEmailOrPhoneExcludingUserAsync(string emailOrPhone, int excludeUserId, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            u => (u.Email == emailOrPhone || u.Phone == emailOrPhone) && u.UserId != excludeUserId,
            cancellationToken);
    }
}
