using SQLiteBrowser;
using TurfApp.MVVM.ViewModel;
using VictuzMobileApp.MVVM.Model;


namespace TurfApp.MVVM.View;

public partial class HomePage : ContentPage
{
	private readonly WeatherService _weatherService;
	public HomePage()
	{
		InitializeComponent();
		_weatherService = new WeatherService();
		BindingContext = new HomePageViewModel();

		LoadWeatherData();
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

	private async Task LoadWeatherData()
	{
		var weather = await _weatherService.GetWeatherAsync();

		if (weather != null)
		{

			WeatherLabel.Text = $"Weer in {weather.Name}: {weather.WeatherDescription}";
		}
		else
		{
			WeatherLabel.Text = "Kan weergegevens niet ophalen.";
		}
	}
}