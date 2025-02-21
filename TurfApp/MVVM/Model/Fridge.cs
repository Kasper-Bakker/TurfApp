using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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