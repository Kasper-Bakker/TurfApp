using System;
using System.Collections.Generic;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace TurfApp.MVVM.Model
{
	public class User
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public string Name { get; set; }

		public decimal Balance { get; set; }
	}
}