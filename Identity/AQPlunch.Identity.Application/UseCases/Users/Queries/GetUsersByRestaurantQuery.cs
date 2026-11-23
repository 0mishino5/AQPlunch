using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Queries;

public class GetUsersByRestaurantQuery : IRequest<List<User>>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetUsersByRestaurantQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersByRestaurantQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersByRestaurantQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Users.ObtenerPorRestaurante(request.RestaurantId);
    }
}
