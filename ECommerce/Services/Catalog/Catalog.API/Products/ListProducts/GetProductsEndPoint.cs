﻿
using System.Threading;

namespace Catalog.API.Products.ListProducts
{
	//request
	public record GetProductsRequest(int? PageNumber=1,int? PageSize=10);
	//response
	public record GetProductsResponse(IEnumerable<Product> Products);
	public class GetProductsEndPoint : ICarterModule

	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/products", async ([AsParameters] GetProductsRequest request,ISender sender) => 
			{
				var query = request.Adapt<GetProductsQuery>();
				var result = await sender.Send(query);
			     var response=result.Adapt<GetProductsResponse>();
				return Results.Ok(response);
			}).WithName("GetProducts").Produces<GetProductsResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Get Products").WithDescription("Get Products");

			
		}
	}
}
