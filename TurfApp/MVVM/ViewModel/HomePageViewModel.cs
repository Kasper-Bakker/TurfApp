using System.Windows.Input;
using TurfApp.MVVM.View;
using TurfApp.MVVM.Data;

namespace TurfApp.MVVM.ViewModel
{
	public class HomePageViewModel
	{
		public ICommand NavigateCommand { get; }

		public HomePageViewModel()
		{
			NavigateCommand = new Command<string>(Navigate);
		}

		private async void Navigate(string pageName)
		{
			Page page = pageName switch
			{
				"HomePage" => new HomePage(),
				"FridgePage" => new FridgePage(App.Database),
				"ShoppingListPage" => new ShoppingListPage(),
				_ => null
			};

			if (page != null)
			{
				await Application.Current.MainPage.Navigation.PushAsync(page);
			}
		}
	}
}