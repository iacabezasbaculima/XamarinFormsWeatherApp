using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Xml.Linq;
using System.Net;
using WeatherApp.Models;

namespace WeatherApp.Services
{
	public class OpenWeatherMapService
	{
		private const string _APP_KEY = "YOUR_KEY";
		private HttpClient _client;

		public enum QueryType { SINGLE_DAY, FIVE_DAYS }	
		public OpenWeatherMapService()
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
		}
		/// <summary>
		/// Fetch weather forecast for today, tomorrow and aftertomorrow every 3 hours
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(string location)
		{
			if (location == null) throw new ArgumentNullException("Location can't be null.");
			if (location == string.Empty) throw new ArgumentException("Location can't be an empty string.");

			var query = $"forecast?q={location}&type=accurate&units=metric&mode=xml&appid={_APP_KEY}";

			var response = await _client.GetAsync(query);

			switch (response.StatusCode)
			{
				case HttpStatusCode.Unauthorized:
					throw new Exception("Invalid API key.");
				case HttpStatusCode.NotFound:
					throw new Exception("Location not found.");
				case HttpStatusCode.OK:
					var s = await response.Content.ReadAsStringAsync();
					var x = XElement.Load(new StringReader(s));

					var data = x.Descendants("time").Select(w => new WeatherForecast
					{
						City = w.Parent.Parent.Element("location").Element("name").Value,
						Country = w.Parent.Parent.Element("location").Element("country").Value,
						UTCOffsetInSecs = int.Parse(w.Parent.Parent.Element("location").Element("timezone").Value),
						Date = DateTime.Parse(w.Attribute("from").Value),
						SunSet = DateTime.Parse(w.Parent.Parent.Element("sun").Attribute("set").Value.Substring(11, 8)),
						SunRise = DateTime.Parse(w.Parent.Parent.Element("sun").Attribute("rise").Value.Substring(11, 8)),
						Description = w.Element("symbol").Attribute("name").Value,
						ImageId = w.Element("symbol").Attribute("var").Value,
						WindDescription = w.Element("windSpeed").Attribute("name").Value,
						WindDirection = w.Element("windDirection").Attribute("code").Value,
						WindSpeed = double.Parse(w.Element("windSpeed").Attribute("mps").Value),
						Temperature = double.Parse(w.Element("temperature").Attribute("value").Value),
						MaxTemperature = double.Parse(w.Element("temperature").Attribute("max").Value),
						MinTemperature = double.Parse(w.Element("temperature").Attribute("min").Value),
						Pressure = int.Parse(w.Element("pressure").Attribute("value").Value),
						Humidity = int.Parse(w.Element("humidity").Attribute("value").Value),
						CloudDescription = w.Element("clouds").Attribute("value").Value,
						CloudValue = int.Parse(w.Element("clouds").Attribute("all").Value)
					});
					return data;
				default:
					throw new NotImplementedException(response.StatusCode.ToString());
			}
		}
		/// <summary>
		/// Fetch weather forecast for today
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public async Task<WeatherForecast> GetSingleForecastAsync(string location)
		{
			if (location == null) throw new ArgumentNullException("Location can't be null.");
			if (location == string.Empty) throw new ArgumentException("Location can't be an empty string.");

			var query = $"weather?q={location}&type=accurate&units=metric&mode=xml&appid={_APP_KEY}";
			var response = await _client.GetAsync(query);

			switch (response.StatusCode)
			{
				case HttpStatusCode.Unauthorized:
					throw new Exception("Invalid API key.");
				case HttpStatusCode.NotFound:
					throw new Exception("Location not found.");
				case HttpStatusCode.OK:
					var s = await response.Content.ReadAsStringAsync();
					var x = XElement.Load(new StringReader(s));

					WeatherForecast forecast = new WeatherForecast
					{
						City = x.Element("city").Attribute("name").Value,
						Country = x.Element("city").Element("country").Value,
						Date = DateTime.Parse(x.Element("lastupdate").Attribute("value").Value),
						SunSet = DateTime.Parse(x.Element("city").Element("sun").Attribute("set").Value),
						SunRise = DateTime.Parse(x.Element("city").Element("sun").Attribute("rise").Value),
						Description = x.Element("weather").Attribute("value").Value,
						Temperature = double.Parse(x.Element("temperature").Attribute("value").Value),
						MaxTemperature = double.Parse(x.Element("temperature").Attribute("max").Value),
						MinTemperature = double.Parse(x.Element("temperature").Attribute("min").Value),
						WindSpeed = double.Parse(x.Element("wind").Element("speed").Attribute("value").Value),
						Humidity = int.Parse(x.Element("humidity").Attribute("value").Value),
						Pressure = int.Parse(x.Element("pressure").Attribute("value").Value),
						IconId = int.Parse(x.Element("weather").Attribute("number").Value),
						ImageId = x.Element("weather").Attribute("icon").Value
					};
					return forecast;
				default:
					throw new NotImplementedException(response.StatusCode.ToString());
			}
		}
	}
}
