using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TurfApp.MVVM.Model
{
	public class User
	{

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string Name { get; set; }

		[NotNull, Unique]
		public string Email { get; set; }

		[NotNull]
		public string Password { get; set; }

		public decimal Balance { get; set; }

		public bool IsActive { get; set; }
	}
}