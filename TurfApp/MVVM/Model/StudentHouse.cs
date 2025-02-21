using SQLite;
using System.Diagnostics.CodeAnalysis;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.Model
{
	public class StudentHouse
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string Name { get; set; }

		[Ignore]
		public List<User> Users { get; set; }

		[Ignore]
		public List<Fridge> Fridges { get; set; }
	}
}
