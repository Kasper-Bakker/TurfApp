using TurfApp.MVVM.ViewModel;

namespace TurfApp.MVVM.View
{
	public partial class ShoppingListPage : ContentPage
	{
		private readonly ShoppingListViewModel _viewModel;

		public ShoppingListPage()
		{
			InitializeComponent();
			_viewModel = new ShoppingListViewModel();
			BindingContext = _viewModel;
		}
	}
}
