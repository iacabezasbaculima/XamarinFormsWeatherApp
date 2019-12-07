using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace WeatherApp.Services
{
	public class GooglePlacesAPIService<T>
	{
		private static readonly string _API_KEY = "AIzaSyBvtgqqAIbljIvEizq2EgxoQ7SVZNMmzlc";
		// base url: https://maps.googleapis.com/maps/api/place/autocomplete/json?
		
		public async Task<T> GetAsync(string textInput, string token)
		{
			try
			{
				var httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/autocomplete/");

				var query = $"json?input={textInput}&types=geocode&key={_API_KEY}&sessiontoken={token}";
				var json = await httpClient.GetStringAsync(query);

				var taskModels = JsonConvert.DeserializeObject<T>(json);

				return taskModels;
			}
			catch (HttpRequestException e)
			{
				Debug.WriteLine("GetAsync error: " + e.Message);
				return default(T);
			}

		}

	}
}
