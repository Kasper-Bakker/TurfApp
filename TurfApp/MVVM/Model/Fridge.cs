using SQLite;
using System.Diagnostics.CodeAnalysis;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.Model
{

	public class Fridge
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string Name { get; set; }

		[Ignore]
		public List<Product> Products { get; set; }
	}
}