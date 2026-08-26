using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(KosaDbContext context) : base(context)
    {
    }
}
