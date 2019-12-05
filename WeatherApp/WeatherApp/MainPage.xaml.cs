using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Xamarin.Forms;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Utils;

namespace WeatherApp
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
		// The name of the city used to fetch weather data
		private string city;
		// An object used to make async API call to fetch weather data
		public OpenWeatherMapService service;
		
		// A collection used to process weather data
		public List<WeatherForecast> forecastData;
		
		// The models that contain all meta data for each day
		public WeatherForecast today;
		public WeatherForecast tomorrow;
		public WeatherForecast afterTomorrow;
		public WeatherForecast fourth;
		public WeatherForecast fifth;

		// The collecton that contains data displayed in UI
		public List<Weather> weathers;

		public MainPage()
		{
			InitializeComponent();
			
			// Initialize OpenWeatherMap service
			service = new OpenWeatherMapService();

			// Initialize weather data list 
			forecastData = new List<WeatherForecast>();

			weathers = new List<Weather>();
			
			this.BindingContext = this;
		}

		public MainPage(string location)
		{

			// Set city
			city = location;

			// Initialize OpenWeatherMap service
			service = new OpenWeatherMapService();

			// Initialize weather data list 
			forecastData = new List<WeatherForecast>();

			weathers = new List<Weather>();

			GetWeather(location);

			InitializeComponent();

			this.BindingContext = this;
		}
		/// <summary>
		/// Execute Http request to fetch weather data asynchronously
		/// </summary>
		/// <param name="cityName"></param>
		public async void GetWeather(string cityName)
		{
			var data = await service.GetForecastAsync(cityName);

			today = data.First();
			forecastData = data.Where(i => i.Date.Day == TimeOfDay.Tomorrow.Day).ToList();

			// Tomorrow
			tomorrow = new WeatherForecast();
			Tuple<int, int> tuple = ProcessData(forecastData, out tomorrow);
			tomorrow.MinTemperature = tuple.Item1;
			tomorrow.MaxTemperature = tuple.Item2;

			forecastData.Clear();

			// AfterTomorrow
			afterTomorrow = new WeatherForecast();
			forecastData = data.Where(i => i.Date.Day == TimeOfDay.AfterTomorrow.Day).ToList();
			tuple = ProcessData(forecastData, out afterTomorrow);
			afterTomorrow.MinTemperature = tuple.Item1;
			afterTomorrow.MaxTemperature = tuple.Item2;

			forecastData.Clear();

			// AfterTomorrow + 1 Day 
			forecastData = data.Where(i => i.Date.Day == TimeOfDay.AfterTomorrow.AddDays(1).Day).ToList();
			fourth = new WeatherForecast();
			tuple = ProcessData(forecastData, out fourth);
			fourth.MinTemperature = tuple.Item1;
			fourth.MaxTemperature = tuple.Item2;

			// AfterTomorrow + 2 Days
			forecastData = data.Where(i => i.Date.Day == TimeOfDay.AfterTomorrow.AddDays(2).Day).ToList();
			fifth = new WeatherForecast();
			tuple = ProcessData(forecastData, out fifth);
			fifth.MinTemperature = tuple.Item1;
			fifth.MaxTemperature = tuple.Item2;

			UpdateUIElements();
		}
		private async void ViewCities_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
		/// <summary>
		/// Update everything displayed in UI with forecast data
		/// </summary>
		private void UpdateUIElements()
		{
			// Today
			lbDate.Text = $"{DateTime.Now.ToString("ddd, d MMMM hh:mm tt", CultureInfo.InvariantCulture)}";
			imgToday.Source = $"w{today.ImageId}.png";
			lbTemp.Text = $"{Math.Round(today.Temperature)}";
			lbDescription.Text = $"{today.Description.First().ToString().ToUpper()}{today.Description.Substring(1)}";
			lbLocation.Text = $"{today.City.ToUpper()}, {today.Country}";
			lbHumidity.Text = $"{today.Humidity}%";
			lbWind.Text = $"{today.WindSpeed} m/s";
			lbPressure.Text = $"{today.Pressure} hPa";
			lbCloudiness.Text = $"{today.CloudValue}%";

			UpdateListView();
		}
		/// <summary>
		/// Updates the ItemSource of the ListView 
		/// </summary>
		private void UpdateListView()
		{
			// Reset ItemSource of ListView element
			WeatherForecastList.ItemsSource = null;
			
			// Clear previous bindind data in collection of weather data
			weathers.Clear();

			weathers.Add(
				new Weather()
				{
					Date = tomorrow.Date.ToString("ddd d"),
					Temp = $"{tomorrow.MaxTemperature}/{tomorrow.MinTemperature}",
					Icon = $"w{tomorrow.ImageId}.png"
				});
			weathers.Add(
				new Weather()
				{
					Date = afterTomorrow.Date.ToString("ddd d"),
					Temp = $"{afterTomorrow.MaxTemperature}/{afterTomorrow.MinTemperature}",
					Icon = $"w{afterTomorrow.ImageId}.png"
				});
			weathers.Add(
				new Weather()
				{
					Date = fourth.Date.ToString("ddd d"),
					Temp = $"{fourth.MaxTemperature}/{fourth.MinTemperature}",
					Icon = $"w{fourth.ImageId}.png"
				});
			weathers.Add(
				new Weather()
				{
					Date = fifth.Date.ToString("ddd d"),
					Temp = $"{fifth.MaxTemperature}/{fifth.MinTemperature}",
					Icon = $"w{fifth.ImageId}.png"
				});
			WeatherForecastList.ItemsSource = weathers;
		}
		/// <summary>
		/// Find the max and min temperature from a set of data for one day
		/// </summary>
		/// <param name="list">The set of data</param>
		/// <param name="day">The returned model that contains the data</param>
		/// <returns></returns>
		private Tuple<int, int> ProcessData(List<WeatherForecast> list, out WeatherForecast day)
		{
			// Find the highest temperature for the day
			var maxTemp = 0;
			list.ForEach(i =>
			{
				if (i.MaxTemperature > maxTemp)
					maxTemp = (int)Math.Floor(i.MaxTemperature);
			});

			// Find the lowest temperature for the day
			var minTemp = 100;
			list.ForEach(j =>
			{
				if (j.MinTemperature < minTemp)
					minTemp = (int)Math.Floor(j.MinTemperature);
			});

			// Assign a WeatherForecast object to tomorrow
			day = list.First();

			return new Tuple<int, int>(minTemp, maxTemp);
		}
	}
	public class Weather
	{
		public string Temp { get; set; }
		public string Date { get; set; }
		public string Icon { get; set; }
	}
}
