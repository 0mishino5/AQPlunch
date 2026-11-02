using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Restaurants.Commands;

public class AddRestaurantCommand : IRequest<Guid>
{
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
}

internal sealed class AddRestaurantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(AddRestaurantCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 150)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 150 caracteres");
        }

        if ((request.Address != null && request.Address.Length > 255) || (request.Phone != null && request.Phone.Length > 20))
        {
            throw new ReglaNegocioException("La direccion tiene como maximo 255 caracteres y el telefono 20");
        }

        Restaurant restaurante = new Restaurant
        {
            RestaurantId = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        await unitOfWork.Repositorio<Restaurant>().Agregar(restaurante);
        await unitOfWork.GuardarCambios();

        return restaurante.RestaurantId;
    }
}
