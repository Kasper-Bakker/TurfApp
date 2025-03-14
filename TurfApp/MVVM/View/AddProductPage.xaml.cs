using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using System.Collections.Generic;
using System.Linq;

namespace TurfApp.MVVM.View
{
	public partial class AddProductPage : ContentPage
	{
		private readonly FridgeDetailPage _fridgePage;
		private readonly Constants _database;
		private int _fridgeId;
		private List<Product> _existingProducts = new List<Product>();

		public AddProductPage(FridgeDetailPage fridgePage, int fridgeId, Constants database)
		{
			InitializeComponent();
			_fridgePage = fridgePage;
			_fridgeId = fridgeId;
			_database = database;

			ProductStockEntry.TextChanged += OnPriceOrStockChanged;
			TotalPriceEntry.TextChanged += OnPriceOrStockChanged;

			LoadExistingProducts();
		}

		private async void LoadExistingProducts()
		{
			_existingProducts = await _database.GetAllAsync<Product>();

			ProductPicker.Items.Add("Nieuw product");

			foreach (var product in _existingProducts)
			{
				ProductPicker.Items.Add(product.Name);
			}

			ProductPicker.SelectedIndex = 0;
		}

		private void OnProductSelected(object sender, EventArgs e)
		{
			if (ProductPicker.SelectedIndex == 0)
			{
				ProductNameEntry.Text = "";
				ProductCategoryEntry.Text = "";
				ProductPriceEntry.Text = "";
				ProductNameEntry.IsEnabled = true;
				ProductCategoryEntry.IsEnabled = true;
			}
			else
			{
				var selectedProduct = _existingProducts[ProductPicker.SelectedIndex - 1];
				ProductNameEntry.Text = selectedProduct.Name;
				ProductCategoryEntry.Text = selectedProduct.Category;
				ProductPriceEntry.Text = selectedProduct.PricePerUnit.ToString("0.00");

				ProductNameEntry.IsEnabled = false;
				ProductCategoryEntry.IsEnabled = false;
			}
		}

		private void OnPriceOrStockChanged(object sender, EventArgs e)
		{
			bool isStockValid = int.TryParse(ProductStockEntry.Text, out int stock);
			bool isTotalPriceValid = decimal.TryParse(TotalPriceEntry.Text, out decimal totalPrice);

			if (isStockValid && isTotalPriceValid && stock > 0)
			{
				decimal pricePerUnit = totalPrice / stock;
				ProductPriceEntry.Text = pricePerUnit.ToString("0.00");
			}
			else
			{
				ProductPriceEntry.Text = string.Empty;
			}
		}

		private async void OnSaveClicked(object sender, EventArgs e)
		{
			if (ProductPicker.SelectedIndex == 0) 
			{
				if (string.IsNullOrWhiteSpace(ProductNameEntry.Text) ||
					string.IsNullOrWhiteSpace(ProductCategoryEntry.Text) ||
					!int.TryParse(ProductStockEntry.Text, out int stock) ||
					!decimal.TryParse(ProductPriceEntry.Text, out decimal price))
				{
					await DisplayAlert("Fout", "Vul alle velden correct in.", "OK");
					return;
				}

				var newProduct = new Product
				{
					FridgeId = _fridgeId,
					Name = ProductNameEntry.Text,
					Category = ProductCategoryEntry.Text,
					Stock = stock,
					PricePerUnit = price
				};

				await _fridgePage.AddProduct(newProduct);
			}
			else 
			{
				var selectedProduct = _existingProducts[ProductPicker.SelectedIndex - 1];

				if (!int.TryParse(ProductStockEntry.Text, out int additionalStock))
				{
					await DisplayAlert("Fout", "Vul een geldig aantal in.", "OK");
					return;
				}

				selectedProduct.Stock += additionalStock;
				await _database.UpdateAsync(selectedProduct);
			}

			await Navigation.PopAsync();
		}
	}
}
