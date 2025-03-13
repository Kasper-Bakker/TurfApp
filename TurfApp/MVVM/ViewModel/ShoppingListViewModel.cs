using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.ViewModel
{
	public class ShoppingListViewModel 
	{
		private readonly Constants _database;
		public ObservableCollection<Product> ShoppingList { get; set; } = new();

		public ShoppingListViewModel()
		{
			_database = App.Database;
			LoadShoppingList();
		}

		private async void LoadShoppingList()
		{
			var products = await _database.GetAllAsync<Product>();
			ShoppingList.Clear();
			foreach (var product in products.Where(p => p.Stock > 0)) 
			{
				ShoppingList.Add(product);
			}
		}

		public async Task AddToShoppingList(Product product)
		{
			var existingProduct = ShoppingList.FirstOrDefault(p => p.Name == product.Name);
			if (existingProduct != null)
			{
				existingProduct.Stock++;
			}
			else
			{
				ShoppingList.Add(new Product
				{
					Name = product.Name,
					Stock = 1
				});
			}

			await Task.CompletedTask;
		}
	}
}
