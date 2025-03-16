using System.Collections.ObjectModel;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.ViewModel;
using SQLiteBrowser;
using TurfApp.MVVM.Data;
using TurfApp.Services;


namespace TurfApp.MVVM.View
{
	public partial class HomePage : ContentPage
	{
		private readonly WeatherService _weatherService;
		private readonly Constants _database;
		public ObservableCollection<Product> StockItems { get; set; } = new ObservableCollection<Product>();

		public HomePage()
		{
			InitializeComponent();
			_weatherService = new WeatherService();
			_database = App.Database;

			BindingContext = this;

			LoadWeatherData();
			LoadStock();

			_database.OnDatabaseUpdated += LoadStock;
		}

		private async void LoadWeatherData()
		{
			var weather = await _weatherService.GetWeatherAsync();
			WeatherLabel.Text = weather != null
				? $"Weer in {weather.Name}: {weather.WeatherDescription}"
				: "Kan weergegevens niet ophalen.";
		}

		private async void LoadStock()
		{
			var products = await _database.GetAllAsync<Product>();

			MainThread.BeginInvokeOnMainThread(() =>
			{
				StockItems.Clear();
				bool lowStock = false;

				foreach (var product in products)
				{
					StockItems.Add(product);
					if (product.Stock < 5)
					{
						lowStock = true;
					}
				}

				LowStockLabel.IsVisible = lowStock;
			});
		}

		private async void OnLogoutButtonClicked(object sender, EventArgs e)
		{
			bool confirmLogout = await DisplayAlert("Uitloggen", "Weet u zeker dat u wilt uitloggen?", "Ja", "Nee");
			if (!confirmLogout) return;

			try
			{
				await App.Database.SetActiveUser(0);
				App.CurrentUser = null;
				await Navigation.PushAsync(new StartPage());
			}
			catch (Exception ex)
			{
				await DisplayAlert("Fout", $"Er is iets misgegaan tijdens het uitloggen: {ex.Message}", "OK");
			}
		}

		private async void OpenDatabaseBrowser(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "TurfApp.db")));
		}
	}
}
