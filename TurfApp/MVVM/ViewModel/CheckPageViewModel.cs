using System.Collections.ObjectModel;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.ViewModel
{
	public class CheckPageViewModel
	{
		private readonly Constants _database;
		public ObservableCollection<UserTransaction> Transactions { get; set; } = new();

		public CheckPageViewModel()
		{
			_database = App.Database;
			LoadTransactions();
		}

		private async void LoadTransactions()
		{
			var transactions = await _database.GetAllAsync<UserTransaction>();
			Transactions.Clear();

			foreach (var transaction in transactions)
			{
				var product = await _database.GetAsync<Product>(transaction.ProductId);
				if (product != null)
				{
					transaction.ProductName = product.Name;
					Transactions.Add(transaction);
				}
			}
		}

		public async Task RefreshTransactions()
		{
			var transactions = await _database.GetAllAsync<UserTransaction>();
			Transactions.Clear();

			foreach (var transaction in transactions)
			{
				var product = await _database.GetAsync<Product>(transaction.ProductId);
				if (product != null)
				{
					Transactions.Add(transaction);
				}
			}
		}

		public async Task AddProductToUser(int userId, Product product)
		{
			var existingTransaction = Transactions.FirstOrDefault(t => t.UserId == userId && t.ProductId == product.Id);

			if (existingTransaction != null)
			{
				existingTransaction.Quantity++;
				await _database.UpdateAsync(existingTransaction);
			}
			else
			{
				var newTransaction = new UserTransaction
				{
					UserId = userId,
					ProductId = product.Id,
					Quantity = 1,
					PricePerUnit = product.PricePerUnit
				};
				await _database.AddAsync(newTransaction);
				Transactions.Add(newTransaction);
			}
		}
		public decimal GetTotalAmount(int userId)
		{
			return Transactions
				.Where(t => t.UserId == userId)
				.Sum(t => t.TotalPrice);
		}
	}
}
