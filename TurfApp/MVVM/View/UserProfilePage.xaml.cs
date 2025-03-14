using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;
using TurfApp.MVVM.Model;
using TurfApp.MVVM.Data;

namespace TurfApp.MVVM.View
{
	public partial class UserProfilePage : ContentPage
	{
		private readonly Constants _database;
		private User _user;

		public UserProfilePage(int userId)
		{
			InitializeComponent();
			_database = App.Database;
			LoadUser(userId);
		}

		private async void LoadUser(int userId)
		{
			_user = await _database.GetAsync<User>(userId) ?? new User();
			BindData();
		}

		private void BindData()
		{
			NameEntry.Text = _user.Name;
			EmailEntry.Text = _user.Email;
			PasswordEntry.Text = _user.Password;

			if (!string.IsNullOrEmpty(_user.ProfilePicture))
			{
				ProfileImage.Source = ImageSource.FromFile(_user.ProfilePicture);
			}
		}
		private async void TakePhoto(object sender, EventArgs e)
		{
			try
			{
				var photo = await MediaPicker.CapturePhotoAsync();
				if (photo == null)
					return;

				string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
				using var stream = await photo.OpenReadAsync();
				using var newStream = File.OpenWrite(localFilePath);
				await stream.CopyToAsync(newStream);

				_user.ProfilePicture = localFilePath;
				ProfileImage.Source = ImageSource.FromFile(localFilePath);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Fout", $"Kan geen foto maken: {ex.Message}", "OK");
			}
		}
		private async void PickPhoto(object sender, EventArgs e)
		{
			try
			{
				var photo = await MediaPicker.PickPhotoAsync();
				if (photo == null)
					return;

				string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
				using var stream = await photo.OpenReadAsync();
				using var newStream = File.OpenWrite(localFilePath);
				await stream.CopyToAsync(newStream);

				_user.ProfilePicture = localFilePath;
				ProfileImage.Source = ImageSource.FromFile(localFilePath);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Fout", $"Kan geen foto selecteren: {ex.Message}", "OK");
			}
		}

		private async void SaveUser(object sender, EventArgs e)
		{
			_user.Name = NameEntry.Text;
			_user.Email = EmailEntry.Text;
			_user.Password = PasswordEntry.Text;

			await _database.UpdateAsync(_user);

			if (Application.Current.MainPage is NavigationPage navPage)
			{
				await navPage.PopAsync(); 
			}
			else
			{
				Application.Current.MainPage = new NavigationPage(new HomePage());
			}
		}
	}
}
