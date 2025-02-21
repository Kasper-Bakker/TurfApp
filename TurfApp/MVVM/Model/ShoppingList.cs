using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
