using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Unit>
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
}

internal sealed class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<UpdateCategoryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category categoria = await unitOfWork.Categories.ObtenerPorId(request.CategoryId) ?? throw new RecursoNoEncontradoException($"No existe la categoria con Id {request.CategoryId}");

        if (!usuarioActual.PuedeAdministrar(categoria.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar las categorias de otro restaurante");
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 100)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 100 caracteres");
        }

        if (request.Name.ToLower() != categoria.Name.ToLower() && await unitOfWork.Categories.ExisteNombre(categoria.RestaurantId, request.Name))
        {
            throw new ReglaNegocioException($"Ya existe la categoria {request.Name} en este restaurante");
        }

        categoria.Name = request.Name;

        unitOfWork.Categories.Actualizar(categoria);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
