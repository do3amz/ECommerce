
using Marten.Linq.SoftDeletes;

namespace Catalog.API.Products.DeleteProduct
{
	public record DeleteProductCommand(Guid Id):ICommand<DeleteCommandResult>;
	public record DeleteCommandResult(bool Success);
	public class DeleteProductValidator:AbstractValidator<DeleteProductCommand>
	{
		public DeleteProductValidator()
		{
			RuleFor(o => o.Id).NotEmpty().WithMessage("Id is Required");
		}
	}
	internal class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteCommandResult>
	{
		public async Task<DeleteCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
		{
			
			session.Delete<Product>(command.Id);
			await session.SaveChangesAsync(cancellationToken);
			return new DeleteCommandResult(true);
		}
	}
}
