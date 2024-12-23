﻿
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.UpdateProduct
{
	public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price):ICommand<UpdateProductResult>;
	public record UpdateProductResult(bool Success);
	public class UpdateProductValidator:AbstractValidator<UpdateProductCommand>
	{
		public UpdateProductValidator()
			{
			RuleFor(o => o.Id).NotEmpty().WithMessage("Id is required");
			RuleFor(o => o.Name).Length(2, 150).WithMessage("Name must between 2 and 150 characters");
			RuleFor(o => o.Price).GreaterThanOrEqualTo(0).WithMessage("price must greater than or equal to 0");
			}
	}
	internal class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
	{
		public  async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
		{
			
			var product=await session.LoadAsync<Product>(command.Id, cancellationToken);
			if (product == null)
			{
				throw new ProductNotFoundException(command.Id);
			}
			product.Price = command.Price;
			product.Name= command.Name;
			product.ImageFile = command.ImageFile;
			product.Description= command.Description;	
			product.Category=command.Category;
			 session.Update(product);
			await session.SaveChangesAsync(cancellationToken);
			return new UpdateProductResult(true);
		}
	}
}
