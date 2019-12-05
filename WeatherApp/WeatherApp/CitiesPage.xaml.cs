using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CitiesPage : ContentPage
	{
		private List<City> cities;

		public CitiesPage()
		{
			InitializeComponent();

			// Initialize connection
			cities = new List<City>();
		}
		/// <summary>
		/// Add a city from user input to the ListView and local database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(entCityName.Text))
				return;

			var inputLocation = entCityName.Text;

			// Check if city name is already in the list
			foreach (var item in cities)
			{
				if (item.Name == inputLocation)
					return;
			}

			City newCity = new City(){ Name = $"{entCityName.Text.First().ToString().ToUpper()}{entCityName.Text.Substring(1)}"};
			cities.Add(newCity);

			// Reset entry text
			entCityName.Text = "";

			// Add city to SQLite local database

			using (var con = new SQLite.SQLiteConnection(App.FilePath))
			{
				//con.CreateTable<City>(); table should be created already OnAppearing
				int rowsAdded = con.Insert(newCity);

			}

			// Reset ItemSource of ListView
			lvCities.ItemsSource = null;
			lvCities.ItemsSource = cities;
		}
		/// <summary>
		/// Open a new page to edit currently saved cities
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void BtnEdit_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EditPage());
		}
		/// <summary>
		/// Display weather data for a city if there is a network connection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void lvCities_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			// Check if there is an Internet connection
			if(CrossConnectivity.Current.IsConnected)
			{
				City selectedCity = (City)e.Item;

				if (selectedCity != null)
				{
					await Navigation.PushAsync(new MainPage(selectedCity.Name));
				}

				lvCities.SelectedItem = null;
			}
			else
			{
				await DisplayAlert("Network not available.", "Turn on WiFi or Mobile Network.", "OK");
			}
		}
		/// <summary>
		/// Load up saved cities from sqlite database and set ListView item source
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();

			using (var con = new SQLite.SQLiteConnection(App.FilePath))
			{
				// Runs a "create table if not exists"
				con.CreateTable<City>();
				
				// Get cities from table
				cities = con.Table<City>().ToList();
				
				// Update ListView
				lvCities.ItemsSource = null;
				lvCities.ItemsSource = cities;
			}

			// Show cursor + keyboard when page is loaded
			entCityName.Focus();
		}
		/// <summary>
		/// Delete all cities stored in database
		/// </summary>
		/// <param name="con"></param>
		private void DeleteDBTableData(SQLite.SQLiteConnection con)
		{
			con.DeleteAll<City>();
		}

		
	}
}