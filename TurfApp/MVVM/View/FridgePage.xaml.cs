using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using System.Threading.Tasks;

namespace TurfApp.MVVM.View
{
	public partial class FridgePage : ContentPage
	{
		private readonly Constants _database;
		public ObservableCollection<Product> Products { get; set; } = new();

		public FridgePage(Constants database)
		{
			InitializeComponent();
			_database = database;
			BindingContext = this;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await LoadProducts();
		}

		private async Task LoadProducts()
		{
			var productsFromDb = await _database.GetAllAsync<Product>();
			Products.Clear();
			foreach (var product in productsFromDb)
			{
				Products.Add(product);
			}
		}

		async void OnAddProductClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AddProductPage(this, _database));
		}

		public async Task AddProduct(Product product)
		{
			await _database.AddAsync(product);
			Products.Add(product);
		}
	}
}
