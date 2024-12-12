using Marten.Schema;

namespace Catalog.API.Data
{
	public class CatalogInitialData : IInitialData
	{
		public async Task Populate(IDocumentStore store, CancellationToken cancellation)
		{
			using var session = store.LightweightSession();
			if (await session.Query<Product>().AnyAsync())
			{
				return;
			}
			session.Store<Product>(GetPreConfigurationProducts());
			await session.SaveChangesAsync();
		}

		private static IEnumerable<Product> GetPreConfigurationProducts() => new List<Product>
		{
			new Product()
			{

				Id = new Guid("1193b701-315f-443d-a68e-837644fc0603"),
				Name = "shakosh",
				Category = ["c7", "c8"],
				Description = "shakosh",
				ImageFile = "shakosh",
				Price = 20
			}
		};
}
