using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.View
{
	public partial class AddProductPage : ContentPage
	{
		private readonly FridgeDetailPage _fridgePage;
		private int _fridgeId;

		public AddProductPage(FridgeDetailPage fridgePage, int fridgeId)
		{
			InitializeComponent();
			_fridgePage = fridgePage;
			_fridgeId = fridgeId;

			ProductStockEntry.TextChanged += OnPriceOrStockChanged;
			TotalPriceEntry.TextChanged += OnPriceOrStockChanged;
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
			await Navigation.PopAsync();
		}
	}
}
