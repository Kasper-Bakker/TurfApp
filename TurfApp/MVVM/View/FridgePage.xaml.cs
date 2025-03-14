using System.Collections.ObjectModel;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;

namespace TurfApp.MVVM.View
{
	public partial class FridgePage : ContentPage
	{
		private ObservableCollection<Fridge> _fridges = new();

		public FridgePage()
		{
			InitializeComponent();
			LoadFridges();
		}

		public async void LoadFridges()
		{
			var fridges = await App.Database.GetAllAsync<Fridge>();
			_fridges.Clear();
			foreach (var fridge in fridges)
			{
				_fridges.Add(fridge);
			}
			FridgeListView.ItemsSource = _fridges;
		}

		private async void OpenFridgePage(object sender, ItemTappedEventArgs e)
		{
			if (e.Item is Fridge fridge)
			{
				await Navigation.PushAsync(new FridgeDetailPage(fridge.Id));
			}
		}
	}
}
