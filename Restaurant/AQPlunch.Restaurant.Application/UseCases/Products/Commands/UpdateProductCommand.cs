using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Commands;

public class UpdateProductCommand : IRequest<Unit>
{
    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}

internal sealed class UpdateProductCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<UpdateProductCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product producto = await unitOfWork.Products.ObtenerPorId(request.ProductId) ?? throw new RecursoNoEncontradoException($"No existe el producto con Id {request.ProductId}");

        if (!usuarioActual.PuedeAdministrar(producto.Category.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar los productos de otro restaurante");
        }

        Category categoria = await unitOfWork.Categories.ObtenerPorId(request.CategoryId) ?? throw new RecursoNoEncontradoException($"No existe la categoria con Id {request.CategoryId}");

        if (categoria.RestaurantId != producto.Category.RestaurantId)
        {
            throw new ReglaNegocioException("La categoria debe ser del mismo restaurante del producto");
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 150)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 150 caracteres");
        }

        if (request.Price < 0 || request.Price > 99999999.99m)
        {
            throw new ReglaNegocioException("El precio debe estar entre 0 y 99999999.99");
        }

        producto.CategoryId = categoria.CategoryId;
        producto.Category = categoria;
        producto.Name = request.Name;
        producto.Description = request.Description;
        producto.Price = request.Price;
        producto.IsAvailable = request.IsAvailable;

        unitOfWork.Products.Actualizar(producto);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
