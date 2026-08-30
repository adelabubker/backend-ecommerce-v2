
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class CartRepository : GenericRepository<Cart>, ICartRepository
{
    public CartRepository(KosaDbContext context)
        : base(context)
    {
    }

    public async Task<Cart?> GetActiveCartByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.ProductImages)
            .FirstOrDefaultAsync(
                c => c.UserId == userId && !c.IsOrdered,
                cancellationToken);
    }

    public void DeleteCartItem(CartItem item)
    {
        Context.Set<CartItem>().Remove(item);
    }
}