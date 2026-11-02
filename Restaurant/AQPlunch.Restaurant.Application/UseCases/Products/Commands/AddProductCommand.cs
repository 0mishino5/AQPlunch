using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Commands;

public class AddProductCommand : IRequest<Guid>
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

internal sealed class AddProductCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<AddProductCommand, Guid>
{
    public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        Category categoria = await unitOfWork.Categories.ObtenerPorId(request.CategoryId) ?? throw new RecursoNoEncontradoException($"No existe la categoria con Id {request.CategoryId}");

        if (!usuarioActual.PuedeAdministrar(categoria.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes agregar productos a otro restaurante");
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 150)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 150 caracteres");
        }

        if (request.Price < 0 || request.Price > 99999999.99m)
        {
            throw new ReglaNegocioException("El precio debe estar entre 0 y 99999999.99");
        }

        Product producto = new Product
        {
            ProductId = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            IsAvailable = true,
            CreatedAt = DateTime.Now
        };

        await unitOfWork.Products.Agregar(producto);
        await unitOfWork.GuardarCambios();

        return producto.ProductId;
    }
}
