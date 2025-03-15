using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurfApp.MVVM.Data;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.ViewModel;

namespace TurfApp.MVVM.View
{
	public partial class ShoppingListPage : ContentPage
	{
		private readonly ShoppingListViewModel _viewModel;
		private readonly Constants _database;
		private List<User> _users = new List<User>();

		public ShoppingListPage()
		{
			InitializeComponent();
			_viewModel = new ShoppingListViewModel();
			BindingContext = _viewModel;
			_database = App.Database;

			LoadUsers();
		}

		private async void LoadUsers()
		{
			_users = await _database.GetAllAsync<User>(); 
			SetCurrentResponsibleUser();
		}

		private void SetCurrentResponsibleUser()
		{
			if (_users == null || _users.Count == 0)
			{
				ResponsibleUserLabel.Text = "Geen gebruikers gevonden.";
				return;
			}

			int weekNumber = GetCurrentWeekNumber();
			User responsibleUser = _users[weekNumber % _users.Count];

			ResponsibleUserLabel.Text = $"Deze week doet {responsibleUser.Name} de boodschappen!";
		}

		private int GetCurrentWeekNumber()
		{
			DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
			return (DateTime.Now - startOfYear).Days / 7;
		}
	}
}
