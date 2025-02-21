using Microsoft.Maui.Controls;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.View;

namespace TurfApp;

public partial class App : Application
{
	public static Constants Database { get; private set; }
	public static User CurrentUser { get; set; }

	public App()
	{
		InitializeComponent();
		InitializeDatabase();

		CurrentUser = Database.GetActiveUser();

		if (CurrentUser != null)
		{
			MainPage = new NavigationPage(new HomePage());
		}
		else
		{
			MainPage = new NavigationPage(new StartPage());
		}
	}

	private void InitializeDatabase()
	{
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "TurfApp.db");
		Database = new Constants(dbPath);
	}

	public static User AdminUser { get; } = new User
	{
		Id = -1,
		Name = "Admin",
		Email = "admin@admin.com",
		Password = "admin123",
		IsActive = false
	};
}