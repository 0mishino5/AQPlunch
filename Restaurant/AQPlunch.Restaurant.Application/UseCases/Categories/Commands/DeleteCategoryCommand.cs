using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Categories.Commands;

public class DeleteCategoryCommand : IRequest<Unit>
{
    public Guid CategoryId { get; set; }
}

internal sealed class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category categoria = await unitOfWork.Categories.ObtenerPorId(request.CategoryId) ?? throw new RecursoNoEncontradoException($"No existe la categoria con Id {request.CategoryId}");

        if (!usuarioActual.PuedeAdministrar(categoria.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes eliminar las categorias de otro restaurante");
        }

        if (await unitOfWork.Products.ExistenEnCategoria(request.CategoryId))
        {
            throw new ReglaNegocioException("No se puede eliminar una categoria que tiene productos");
        }

        unitOfWork.Categories.Eliminar(categoria);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
