using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurfApp.MVVM.Model;

namespace TurfApp.MVVM.Data
{
	public class Constants
	{
		private readonly SQLiteAsyncConnection _database;

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

		public Task<int> AddAsync<T>(T item) where T : new()
		{
			return _database.InsertAsync(item);
		}

		public Task<int> UpdateAsync<T>(T item) where T : new()
		{
			return _database.UpdateAsync(item);
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
	}
}
