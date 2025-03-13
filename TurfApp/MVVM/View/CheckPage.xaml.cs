using TurfApp.MVVM.ViewModel;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using System.Transactions;

namespace TurfApp.MVVM.View
{
	public partial class CheckPage : ContentPage
	{
		private readonly CheckPageViewModel _viewModel;
		private int _currentUserId = 1;

		public CheckPage()
		{
			InitializeComponent();
			_viewModel = new CheckPageViewModel();
			BindingContext = _viewModel;
			LoadUserTotal();
		}

		private async void LoadUserTotal()
		{
			await _viewModel.RefreshTransactions();
			TotalAmountLabel.Text = $"Totaal: € {_viewModel.GetTotalAmount(_currentUserId):0.00}";
		}
	}
}
