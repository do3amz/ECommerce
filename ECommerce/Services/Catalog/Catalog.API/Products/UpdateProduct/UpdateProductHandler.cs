
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.UpdateProduct
{
	public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price):ICommand<UpdateProductResult>;
	public record UpdateProductResult(bool Success);
	internal class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
	{
		public  async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation("UpdateProductHandler.Handle called with {@Command}", command);
			var product=await session.LoadAsync<Product>(command.Id, cancellationToken);
			if (product == null)
			{
				throw new ProductNotFoundException();
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
