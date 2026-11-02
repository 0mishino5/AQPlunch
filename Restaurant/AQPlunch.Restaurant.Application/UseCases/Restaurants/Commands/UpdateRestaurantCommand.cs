using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Restaurants.Commands;

public class UpdateRestaurantCommand : IRequest<Unit>
{
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}

internal sealed class UpdateRestaurantCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<UpdateRestaurantCommand, Unit>
{
    public async Task<Unit> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar otro restaurante");
        }

        Restaurant restaurante = await unitOfWork.Repositorio<Restaurant>().ObtenerPorId(request.RestaurantId) ?? throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 150)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 150 caracteres");
        }

        if ((request.Address != null && request.Address.Length > 255) || (request.Phone != null && request.Phone.Length > 20))
        {
            throw new ReglaNegocioException("La direccion tiene como maximo 255 caracteres y el telefono 20");
        }

        restaurante.Name = request.Name;
        restaurante.Address = request.Address;
        restaurante.Phone = request.Phone;
        restaurante.IsActive = request.IsActive;

        unitOfWork.Repositorio<Restaurant>().Actualizar(restaurante);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
