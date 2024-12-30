using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Ordering.Domain.ValueObjects
{
	public record OrderName
	{
		private const int DefaultLength = 5; 
		public string Value { get;  }
		private OrderName(string value) => Value = value;
		public static OrderName of(string value)
		{
			ArgumentException.ThrowIfNullOrEmpty(value);
			ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

			return new OrderName(value);
		}
		

	}
}
