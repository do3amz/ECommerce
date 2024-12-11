

namespace Catalog.API.Products.AddProduct
{
	public record CreateProductCommand(string name,List<string> category,string description,string imageFile,decimal price):ICommand<CreateProductResult>;
	public record CreateProductResult(Guid Id);
	public class CreateProductValidator:AbstractValidator<CreateProductCommand>
	{
		public CreateProductValidator() 
		{
			RuleFor(o => o.name).NotEmpty().WithMessage("Name is required");
			RuleFor(o => o.price).GreaterThanOrEqualTo(0).WithMessage("Price must greater than or equal to 0");
			RuleFor(o => o.category).NotEmpty().WithMessage("Category is required");
			RuleFor(o => o.imageFile).NotEmpty().WithMessage("Image File is required");
		}	
	}




	internal class CreateProductCommandHandler(IDocumentSession session,ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
	{
		public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
		{
			#region validator
            
			#endregion
			logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);
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
