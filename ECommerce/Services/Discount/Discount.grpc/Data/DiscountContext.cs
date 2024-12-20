using Discount.grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.grpc.Data
{
	public class DiscountContext : DbContext
	{
		public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
		{
		}
		public DbSet<Coupon> Coupons { get; set; } = default!;
	}
}
