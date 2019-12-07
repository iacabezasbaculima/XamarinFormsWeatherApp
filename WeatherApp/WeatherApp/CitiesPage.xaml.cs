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
		/// Go to search page 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnAdd_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SearchPage());
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
				//con.CreateTable<City>();
				
				// Get cities from table
				cities = con.Table<City>().ToList();
				
				// Update ListView
				lvCities.ItemsSource = null;
				lvCities.ItemsSource = cities;
			}

		}
	}
}