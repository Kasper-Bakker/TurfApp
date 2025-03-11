using System.Collections.ObjectModel;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.ViewModel
{
	public class ShoppingListViewModel 
	{
		private readonly Constants _database;
		public ObservableCollection<Product> ShoppingList { get; } = new();

		public ShoppingListViewModel()
		{
			_database = App.Database;
			LoadShoppingList();
		}

		private async void LoadShoppingList()
		{
			var products = await _database.GetAllAsync<Product>();

			var neededProducts = products.Where(p => p.Stock < p.MinimumStock);

			ShoppingList.Clear();
			foreach (var product in neededProducts)
			{
				ShoppingList.Add(product);
			}
		}

		public async Task UpdateShoppingList()
		{
			LoadShoppingList();
		}
	}
}
