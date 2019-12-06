using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace WeatherApp.Services
{
	public class GooglePlaceAPIService<T>
	{
		public static readonly string _API_KEY = "AIzaSyBvtgqqAIbljIvEizq2EgxoQ7SVZNMmzlc";
		//https://maps.googleapis.com/maps/api/place/autocomplete/json?input=1600+Amphitheatre&key=AIzaSyBvtgqqAIbljIvEizq2EgxoQ7SVZNMmzlc&sessiontoken=1234567890

		public async Task<T> GetAsync(string WebServiceUrl)
		{
			try
			{
				var httpClient = new HttpClient();

				var json = await httpClient.GetStringAsync(WebServiceUrl);

				var taskModels = JsonConvert.DeserializeObject<T>(json);

				return taskModels;
			}
			catch (Exception e)
			{
				Debug.WriteLine("GetAsync error: " + e.Message);
				return default(T);
			}

		}


	}
}
