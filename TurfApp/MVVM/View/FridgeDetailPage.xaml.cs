using System.Collections.ObjectModel;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.ViewModel;

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

		private async void OnTakeProductClicked(object sender, EventArgs e)
		{
			if (sender is Button button && button.BindingContext is Product product)
			{
				if (product.Stock > 0)
				{
					bool isConfirmed = await DisplayAlert("Bevestiging",
						$"Wil je een {product.Name} pakken?", "Ja", "Annuleren");

					if (isConfirmed)
					{
						product.Stock--;
						await _database.UpdateAsync(product);
						LoadProducts();

						var checkPageViewModel = new CheckPageViewModel();
						await checkPageViewModel.AddProductToUser(1, product); 

						var shoppingListViewModel = new ShoppingListViewModel();
						await shoppingListViewModel.AddToShoppingList(product);
					}
				}
				else
				{
					await DisplayAlert("Leeg", $"{product.Name} is op!", "OK");
				}
			}
		}

	}
}
