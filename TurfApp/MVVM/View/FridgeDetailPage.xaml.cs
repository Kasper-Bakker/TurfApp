using System.Collections.ObjectModel;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.View
{
	public partial class FridgeDetailPage : ContentPage
	{
		private readonly Constants _database;
		private ObservableCollection<Product> _products = new();
		private int _fridgeId;

		public FridgeDetailPage(int fridgeId)
		{
			InitializeComponent();
			_database = App.Database;
			_fridgeId = fridgeId;

			ProductListView.ItemsSource = _products;
			LoadProducts();
		}

		private async void LoadProducts()
		{
			var products = await _database.GetAllAsync<Product>();
			_products.Clear();

			foreach (var product in products.Where(p => p.FridgeId == _fridgeId))
			{
				_products.Add(product);
			}
		}

		private async void AddProductButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AddProductPage(this, _fridgeId));
		}

		public async Task AddProduct(Product newProduct)
		{
			newProduct.FridgeId = _fridgeId;
			await _database.AddAsync(newProduct);
			LoadProducts();
		}
	}
}
