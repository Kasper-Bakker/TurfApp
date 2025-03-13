using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TurfApp.MVVM.Model
{
	public class Product 
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string Category { get; set; }

		[NotNull]
		public string Name { get; set; }

		[NotNull]
		public int Stock { get; set; }

		[NotNull]
		public decimal PricePerUnit { get; set; }

		[NotNull]
		public int FridgeId { get; set; }

		public int MinimumStock { get; set; } = 5;

		public int NeededAmount => MinimumStock - Stock > 0 ? MinimumStock - Stock : 0;
	}

}