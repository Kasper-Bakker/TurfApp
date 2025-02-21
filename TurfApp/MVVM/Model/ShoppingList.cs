using System.Diagnostics.CodeAnalysis;
using SQLite;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.Model
{

	public class ShoppingList
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string ItemName { get; set; }

		[NotNull]
		public int Quantity { get; set; }
	}
}
