using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Restaurants.Commands;

public class DeleteRestaurantCommand : IRequest<Unit>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class DeleteRestaurantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRestaurantCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        Restaurant restaurante = await unitOfWork.Repositorio<Restaurant>().ObtenerPorId(request.RestaurantId) ?? throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");

        unitOfWork.Repositorio<Restaurant>().Eliminar(restaurante);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
