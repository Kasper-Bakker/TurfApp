using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurfApp.MVVM.Model;
using Plugin.LocalNotification;

namespace TurfApp.MVVM.Data
{
	public class Constants
	{
		private readonly SQLiteAsyncConnection _database;
		private readonly HashSet<int> _notifiedProducts = new(); 

		public Constants(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);

			try
			{
				_database.CreateTableAsync<Fridge>().Wait();
				_database.CreateTableAsync<Product>().Wait();
				_database.CreateTableAsync<ShoppingList>().Wait();
				_database.CreateTableAsync<StudentHouse>().Wait();
				_database.CreateTableAsync<User>().Wait();
				_database.CreateTableAsync<UserTransaction>().Wait();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating tables: {ex.Message}");
				Console.WriteLine($"StackTrace: {ex.StackTrace}");
				throw;
			}
		}

		public Task<List<T>> GetAllAsync<T>() where T : new()
		{
			return _database.Table<T>().ToListAsync();
		}

		public async Task<int> AddAsync<T>(T item) where T : new()
		{
			int result = await _database.InsertAsync(item);

			if (item is Product product)
			{
				await CheckStockAndNotify(product);
			}

			return result;
		}

		public async Task<int> UpdateAsync<T>(T item) where T : new()
		{
			int result = await _database.UpdateAsync(item);

			if (item is Product product)
			{
				await CheckStockAndNotify(product);
			}

			return result;
		}

		public Task<int> DeleteAsync<T>(T item) where T : new()
		{
			return _database.DeleteAsync(item);
		}

		public Task<int> AddAllAsync<T>(IEnumerable<T> items) where T : new()
		{
			return _database.InsertAllAsync(items);
		}

		public Task<T> GetAsync<T>(int id) where T : new()
		{
			return _database.FindAsync<T>(id);
		}

		public async Task<List<Fridge>> GetFridgesAsync()
		{
			return await _database.Table<Fridge>().ToListAsync();
		}

		public async Task AddFridgeAsync(Fridge fridge)
		{
			await _database.InsertAsync(fridge);
		}

		public User GetActiveUser()
		{
			try
			{
				return _database.Table<User>().Where(p => p.IsActive).FirstOrDefaultAsync().Result;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error retrieving active user: {ex.Message}");
				return null;
			}
		}

		public async Task SetActiveUser(int userId)
		{
			try
			{
				var allUsers = await _database.Table<User>().ToListAsync();
				foreach (var user in allUsers)
				{
					user.IsActive = false;
					await _database.UpdateAsync(user);
				}

				var activeUser = await _database.Table<User>().Where(p => p.Id == userId).FirstOrDefaultAsync();
				if (activeUser != null)
				{
					activeUser.IsActive = true;
					await _database.UpdateAsync(activeUser);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error setting active user: {ex.Message}");
			}
		}

		private async Task CheckStockAndNotify(Product product)
		{
			if (product.Stock <= 5)
			{
				if (!_notifiedProducts.Contains(product.Id))
				{
					SendNotification(product.Name, product.Stock);
					_notifiedProducts.Add(product.Id); 
				}
			}
			else
			{
				_notifiedProducts.Remove(product.Id);
			}
		}

		private void SendNotification(string productName, int stock)
		{
			var notification = new NotificationRequest
			{
				Title = "Lage voorraad!",
				Description = $"Product '{productName}' heeft nog maar {stock} over. Tijd om boodschappen te doen!",
				ReturningData = "Boodschappenlijst",
				NotificationId = new Random().Next(1000, 9999)
			};

			LocalNotificationCenter.Current.Show(notification);
		}
	}
}
