using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TurfApp.MVVM.Model
{
	public class UserTransaction : INotifyPropertyChanged
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[NotNull]
		public int UserId { get; set; } 

		[NotNull]
		public int ProductId { get; set; } 

		[NotNull]
		public int Quantity { get; set; } 

		[NotNull]
		public decimal PricePerUnit { get; set; }

		[NotNull]
		public string ProductName { get; set; }
		public decimal TotalPrice => Quantity * PricePerUnit; 

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
