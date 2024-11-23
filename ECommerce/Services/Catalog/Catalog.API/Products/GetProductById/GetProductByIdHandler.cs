
namespace Catalog.API.Products.GetProductById
{
	public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdQueryResult>;
	public record GetProductByIdQueryResult(Product Product);
	internal class GetProductByIdQueryHandler (IDocumentSession session,ILogger <GetProductByIdQueryHandler> logger): IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult>
	{
		public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
		{
			logger.LogInformation("GetProductByIdQueryResult.Handle called with {@Query}", query);
			var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
			if(product==null)
			{
				throw new ProductNotFoundException();
			}
			else
			{
				return new GetProductByIdQueryResult(product);
			}

		}
	}
}
