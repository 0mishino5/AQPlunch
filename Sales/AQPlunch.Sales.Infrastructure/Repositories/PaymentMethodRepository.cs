using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Repositories;
using AQPlunch.Sales.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.Sales.Infrastructure.Repositories;

public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(SalesDbContext context) : base(context)
    {
    }

    public override async Task<List<PaymentMethod>> ObtenerTodos()
    {
        return await Consulta()
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<PaymentMethod>> ObtenerActivos()
    {
        return await Consulta()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<PaymentMethod?> ObtenerPorNombre(string nombre)
    {
        return await Consulta()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == nombre.ToLower());
    }
}
