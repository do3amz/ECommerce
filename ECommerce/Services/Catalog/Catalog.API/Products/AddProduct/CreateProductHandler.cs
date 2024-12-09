
using MediatR;
using System.Windows.Input;

namespace Catalog.API.Products.AddProduct
{
	public record CreateProductCommand(string name,List<string> category,string description,string imageFile,decimal price):ICommand<CreateProductResult>;
	public record CreateProductResult(Guid Id);

	internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
	{
		public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
		{
			var product = new Product()
			{
				Name = command.name,
				Category = command.category,
				Description = command.description,
				ImageFile = command.imageFile,
				Price = command.price
			};
			session.Store(product);
			await session.SaveChangesAsync(cancellationToken);
			return new CreateProductResult(product.Id);
		}
	}
}
