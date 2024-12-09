
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductsByCategory
{
	public  record GetProductsByCategoryQuery(string Category):IQuery <GetProductsByCategoryResult>;
	public  record GetProductsByCategoryResult(IEnumerable<Product> Products);
	internal class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
	{
		public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
		{
			logger.LogInformation("GetProductsByCategoryResult.Handle called with {@Query}", query);
			var products = await session.Query<Product>().Where(o => o.Category.Contains(query.Category)).ToListAsync();
			return new GetProductsByCategoryResult(products);
		}
	}
}
