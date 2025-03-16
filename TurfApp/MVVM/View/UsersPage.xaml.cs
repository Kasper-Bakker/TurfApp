using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using System.Collections.ObjectModel;

namespace TurfApp.MVVM.View
{
	public partial class UsersPage : ContentPage
	{
		private readonly Constants _database;
		public ObservableCollection<User> Users { get; set; } = new();

		public UsersPage()
		{
			InitializeComponent();
			_database = App.Database;
			BindingContext = this;
			LoadUsers();
		}

		private async void LoadUsers()
		{
			var users = await _database.GetAllAsync<User>();
			MainThread.BeginInvokeOnMainThread(() =>
			{
				Users.Clear();
				foreach (var user in users)
				{
					Users.Add(user);
				}
			});
		}

		private async void OnDeleteUser(object sender, EventArgs e)
		{
			var button = (Button)sender;
			int userId = (int)button.CommandParameter;

			bool confirm = await DisplayAlert("Verwijderen", "Weet u zeker dat u deze gebruiker wilt verwijderen?", "Ja", "Nee");
			if (!confirm) return;

			var user = await _database.GetAsync<User>(userId);
			if (user != null)
			{
				await _database.DeleteAsync(user);
				LoadUsers();
			}
		}
	}
}
