using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.View
{
	public partial class AddFridgePage : ContentPage
	{
		private FridgePage _fridgePage;

		public AddFridgePage(FridgePage fridgePage = null)
		{
			InitializeComponent();
			_fridgePage = fridgePage;
		}

		private async void OnSaveClicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(FridgeNameEntry.Text))
			{
				await DisplayAlert("Fout", "Voer een geldige naam in.", "OK");
				return;
			}

			var newFridge = new Fridge { Name = FridgeNameEntry.Text };
			await App.Database.AddAsync(newFridge);

			await DisplayAlert("Succes", "Koelkast toegevoegd!", "OK");

			_fridgePage?.LoadFridges();

			await Navigation.PopAsync();
		}
	}
}
