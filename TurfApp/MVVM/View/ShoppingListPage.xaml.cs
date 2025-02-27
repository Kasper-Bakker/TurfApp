using TurfApp.MVVM.ViewModel;

public class ShoppingListPage : ContentPage
{
	public ShoppingListPage()
	{
		BindingContext = new ShoppingListViewModel();
	}
}