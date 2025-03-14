using System;
using System.IO;
using TurfApp.MVVM.View;
using TurfApp.MVVM.Data;
using SQLiteBrowser;

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
			await Navigation.PushAsync(new UsersPage());
		}

		private async void OnAddFridgeButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new AddFridgePage());
		}

		private async void OpenDatabaseBrowser(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new DatabaseBrowserPage(Path.Combine(FileSystem.AppDataDirectory, "TurfApp.db")));
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			App.CurrentUser = null;
			Application.Current.MainPage = new NavigationPage(new StartPage());
		}
	}
}
