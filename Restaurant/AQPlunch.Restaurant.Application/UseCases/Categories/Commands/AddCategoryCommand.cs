using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Categories.Commands;

public class AddCategoryCommand : IRequest<Guid>
{
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = null!;
}

internal sealed class AddCategoryCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<AddCategoryCommand, Guid>
{
    public async Task<Guid> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes agregar categorias a otro restaurante");
        }

        if (!await unitOfWork.Repositorio<Restaurant>().Existe(request.RestaurantId))
        {
            throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 100)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 100 caracteres");
        }

        if (await unitOfWork.Categories.ExisteNombre(request.RestaurantId, request.Name))
        {
            throw new ReglaNegocioException($"Ya existe la categoria {request.Name} en este restaurante");
        }

        Category categoria = new Category
        {
            CategoryId = Guid.NewGuid(),
            RestaurantId = request.RestaurantId,
            Name = request.Name
        };

        await unitOfWork.Categories.Agregar(categoria);
        await unitOfWork.GuardarCambios();

        return categoria.CategoryId;
    }
}
