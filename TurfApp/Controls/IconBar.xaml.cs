using Microsoft.Maui.Controls;

namespace TurfApp.Controls;

public partial class IconBar : ContentView
{
	public IconBar()
	{
		InitializeComponent();

		BindingContext = new TurfApp.MVVM.ViewModel.HomePageViewModel();
	}
}
