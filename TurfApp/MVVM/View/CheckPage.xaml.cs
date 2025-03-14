using TurfApp.MVVM.ViewModel;

namespace TurfApp.MVVM.View
{
	public partial class CheckPage : ContentPage
	{
		private readonly CheckPageViewModel _viewModel;

		public CheckPage(int v)
		{
			InitializeComponent();
			_viewModel = new CheckPageViewModel();
			BindingContext = _viewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await _viewModel.RefreshTransactions();
		}
	}
}