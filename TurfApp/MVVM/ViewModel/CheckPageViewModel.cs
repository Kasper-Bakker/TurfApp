using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TurfApp.MVVM.ViewModel
{
	public class CheckPageViewModel : INotifyPropertyChanged
	{
		private readonly Constants _database;
		private ObservableCollection<UserTransaction> _transactions = new();
		private decimal _totalAmount;

		public event PropertyChangedEventHandler? PropertyChanged;

		public ObservableCollection<UserTransaction> Transactions
		{
			get => _transactions;
			set
			{
				_transactions = value;
				OnPropertyChanged();
			}
		}

		public decimal TotalAmount
		{
			get => _totalAmount;
			set
			{
				_totalAmount = value;
				OnPropertyChanged();
			}
		}

		public ICommand PayCommand { get; }

		public CheckPageViewModel()
		{
			_database = App.Database;
			LoadTransactions();

			// Betaalknop
			PayCommand = new Command(async () => await Pay());
		}

		private async void LoadTransactions()
		{
			await RefreshTransactions();
		}

		public async Task AddProductToCheck(Product product)
		{
			var allTransactions = await _database.GetAllAsync<UserTransaction>();
			var existingTransaction = allTransactions.FirstOrDefault(t => t.ProductId == product.Id);

			if (existingTransaction != null)
			{
				existingTransaction.Quantity++;
				await _database.UpdateAsync(existingTransaction);
			}
			else
			{
				var newTransaction = new UserTransaction
				{
					ProductId = product.Id,
					ProductName = product.Name,
					Quantity = 1,
					PricePerUnit = product.PricePerUnit
				};

				await _database.AddAsync(newTransaction);
			}

			await RefreshTransactions();
		}

		public async Task RefreshTransactions()
		{
			var transactions = await _database.GetAllAsync<UserTransaction>();
			Transactions.Clear();
			decimal total = 0;

			foreach (var transaction in transactions)
			{
				if (string.IsNullOrEmpty(transaction.ProductName))
				{
					var product = await _database.GetAsync<Product>(transaction.ProductId);
					if (product != null)
					{
						transaction.ProductName = product.Name;
						await _database.UpdateAsync(transaction);
					}
					else
					{
						continue;
					}
				}

				Transactions.Add(transaction);
				total += transaction.Quantity * transaction.PricePerUnit;
			}

			TotalAmount = total;
			OnPropertyChanged(nameof(Transactions));
			OnPropertyChanged(nameof(TotalAmount));
		}

		public async Task Pay()
		{
			var confirmation = await Application.Current.MainPage.DisplayAlert(
				"Bevestiging",
				"Weet je zeker dat je de rekening wilt betalen?",
				"Ja",
				"Nee"
			);

			if (!confirmation)
				return; 

			await _database.DeleteAllAsync<UserTransaction>(); 
			await RefreshTransactions(); 

			OnPropertyChanged(nameof(Transactions));
			OnPropertyChanged(nameof(TotalAmount));

			await Application.Current.MainPage.DisplayAlert(
				"Betaling Geslaagd",
				"De rekening is succesvol betaald!",
				"OK"
			);
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
