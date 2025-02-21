using SQLiteBrowser;
using System.IO;

namespace TurfApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private async void OnViewOrDeleteParticipantsClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TurfApp.MVVM.View.UsersPage());
		}

		private async void OpenDatabaseBrowser(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "TurfApp.db")));
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			App.CurrentUser = null;

			Application.Current.MainPage = new NavigationPage(new TurfApp.MVVM.View.StartPage());
		}
	}
}
