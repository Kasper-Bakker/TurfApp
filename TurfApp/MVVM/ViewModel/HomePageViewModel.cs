using System.Windows.Input;
using TurfApp.MVVM.View;
using TurfApp.MVVM.Model;
using System.Collections.ObjectModel;

namespace TurfApp.MVVM.ViewModel
{
	public class HomePageViewModel
	{
		public ICommand NavigateCommand { get; }
		private ObservableCollection<Fridge> _fridges;


		public HomePageViewModel()
		{
			NavigateCommand = new Command<string>(Navigate);
			_fridges = new ObservableCollection<Fridge>();
		}

		private int GetCurrentUserId()
		{
			return 1;
		}


		private async void Navigate(string pageName)
		{
			Page page = pageName switch
			{
				"HomePage" => new HomePage(),
				"FridgePage" => new FridgePage(),
				"ShoppingListPage" => new ShoppingListPage(),
				"CheckPage" => new CheckPage(GetCurrentUserId()),
				"UserProfilePage" => new UserProfilePage(GetCurrentUserId()),
				_ => null
			};

			if (page != null)
			{
				await Application.Current.MainPage.Navigation.PushAsync(page);
			}
		}
	}
}
