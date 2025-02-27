using Microsoft.Maui.Controls;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using System;
using System.Threading.Tasks;

namespace TurfApp.MVVM.View
{
	public partial class AddProductPage : ContentPage
	{
		private readonly FridgePage _fridgePage;
		private readonly Constants _database;

		public AddProductPage(FridgePage fridgePage, Constants database)
		{
			InitializeComponent();
			_fridgePage = fridgePage;
			_database = database;
		}

		async void OnSaveClicked(object sender, EventArgs e)
		{
			string category = CategoryEntry.Text;
			string name = ProductNameEntry.Text;
			bool isStockValid = int.TryParse(ProductStockEntry.Text, out int stock);
			bool isPriceValid = decimal.TryParse(PricePerUnitEntry.Text, out decimal price);

			if (!string.IsNullOrWhiteSpace(category) &&
				!string.IsNullOrWhiteSpace(name) &&
				isStockValid && isPriceValid)
			{
				var newProduct = new Product
				{
					Category = category,
					Name = name,
					Stock = stock,
					PricePerUnit = price
				};

				await _fridgePage.AddProduct(newProduct);
				await Navigation.PopAsync();
			}
			else
			{
				await DisplayAlert("Fout", "Voer een geldige categorie, naam, aantal en prijs in.", "OK");
			}
		}
	}
}
