﻿
using System.Threading;

namespace Catalog.API.Products.ListProducts
{
	//request
	//response
	public record GetProductsResponse(IEnumerable<Product> Products);
	public class GetProductsEndPoint : ICarterModule

	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/products", async (ISender sender) => 
			{
				var result = await sender.Send(new GetProductsQuery());
			     var response=result.Adapt<GetProductsResponse>();
				return Results.Ok(response);
			}).WithName("GetProducts").Produces<GetProductsResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Get Products").WithDescription("Get Products");

			
		}
	}
}