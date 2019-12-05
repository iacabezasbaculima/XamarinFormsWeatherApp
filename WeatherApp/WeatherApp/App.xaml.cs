using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
	public partial class App : Application
	{
		public static string FilePath;

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage( new CitiesPage());
		}
		
		public App(string filePath)
		{
			InitializeComponent();

			FilePath = filePath;

			MainPage = new NavigationPage(new CitiesPage());

		}
		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
