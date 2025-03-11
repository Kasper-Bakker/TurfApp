using TurfApp.MVVM.Model;
using TurfApp.MVVM.View;

namespace TurfApp.MVVM.View
{
	public partial class FridgePage : ContentPage
	{
		public FridgePage()
		{
			InitializeComponent();
		}

		private async void OpenFridgePage(object sender, EventArgs e)
		{
			if (sender is Button button && int.TryParse(button.CommandParameter.ToString(), out int fridgeId))
			{
				await Navigation.PushAsync(new FridgeDetailPage(fridgeId));
			}
		}
	}
}
