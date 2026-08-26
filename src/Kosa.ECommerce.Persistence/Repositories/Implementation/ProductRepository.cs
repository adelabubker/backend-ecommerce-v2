using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(KosaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.IsActive)
            .OrderBy(p => p.ProductName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdWithImagesAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.ProductName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Where(p => p.IsActive && p.ProductName.Contains(name))
            .OrderBy(p => p.ProductName)
            .ToListAsync(cancellationToken);
    }
}
