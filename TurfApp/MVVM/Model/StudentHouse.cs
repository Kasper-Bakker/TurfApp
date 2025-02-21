using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

