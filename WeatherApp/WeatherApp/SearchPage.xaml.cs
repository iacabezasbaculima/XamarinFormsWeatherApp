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
	public partial class SearchPage : ContentPage
	{
		GooglePlacesAPIService<Places.RootObject> service;

		List<Places.Prediction> results;
		
		public SearchPage()
		{
			InitializeComponent();

			SearchBar.Focus();

			service = new GooglePlacesAPIService<Places.RootObject>();

			results = new List<Places.Prediction>();

		}
		/// <summary>
		/// Execute API call to fetch city predictions every other character
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (SearchBar.Text.Length % 2 == 0 || SearchBar.Text.Length % 2 == 1)
			{
				LVResults.ItemsSource = null;
				results.Clear();

				var data = await service.GetAsync(SearchBar.Text, "");

				var temp = data.predictions;

				data.predictions.ForEach(i => results.Add(i));
			
				LVResults.ItemsSource = results;
			}
		}
		/// <summary>
		/// Gets selected prediction from a list view and adds it to database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void LSResults_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			Places.Prediction selectedItem = (Places.Prediction)e.Item;

			if (selectedItem != null)
			{
				City newCity = new City() { Name = $"{selectedItem.structured_formatting.main_text}, {selectedItem.terms.Last().value}" };
				
				using (var con = new SQLite.SQLiteConnection(App.FilePath))
				{
					int rowsAdded = con.Insert(newCity);

					var check = con.Table<City>().ToList();
				}

				// Go back to Cities page
				await Navigation.PushAsync(new MainPage(newCity.Name));
			}
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

			SearchBar.Focus();
		}
	}
}