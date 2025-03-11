using Microsoft.Maui.Controls;
using TurfApp.MVVM.Model;
using System;
using System.Threading.Tasks;

namespace TurfApp.MVVM.View
{
	public partial class AddFridgePage : ContentPage
	{
		public AddFridgePage()
		{
			InitializeComponent();
		}

		private async void OnSaveClicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(FridgeNameEntry.Text))
			{
				await DisplayAlert("Fout", "Voer een geldige naam in.", "OK");
				return;
			}

			var newFridge = new Fridge { Name = FridgeNameEntry.Text };
			await App.Database.AddFridgeAsync(newFridge);

			await DisplayAlert("Succes", "Koelkast toegevoegd!", "OK");
			await Navigation.PopAsync();
		}
	}
}
