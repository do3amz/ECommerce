using Discount.grpc.Data;
using Discount.grpc.Models;
using Discount.grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.grpc.Services
{
	public class DiscountService(DiscountContext discountContext,ILogger<DiscountService> logger):DiscountProtoService.DiscountProtoServiceBase
	{
		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon= await discountContext.Coupons.FirstOrDefaultAsync(o=>o.ProductName==request.ProductName);
			if (coupon is null)
			{
				coupon = new Coupon
				{
					ProductName = "No Discount",
					Amount = 0,
					Description = "No description"

				};
			}
				logger.LogInformation("Discount is retreived for product name :{productName},Amount:{amount}",coupon.ProductName,coupon.Amount);
				var couponModel = coupon.Adapt<CouponModel>();
				return couponModel;
			
		}
		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var coupon = request.Coupon.Adapt<Coupon>();
			if (coupon is null)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
			}
			discountContext.Coupons.Add(coupon);
			await discountContext.SaveChangesAsync();
			logger.LogInformation("Discount is successfully created.ProductName :{ProductName}",coupon.ProductName);
			var couponModel = coupon.Adapt<CouponModel>();
			return couponModel;
		}
		public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			return base.UpdateDiscount(request, context);
		}
		public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			return base.DeleteDiscount(request, context);
		}
	}
}
