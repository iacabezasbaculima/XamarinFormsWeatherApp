using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
	public class CitiesViewModel
	{
		public ObservableCollection<City> Cities { get; set; }
		public Command<City> RemoveCommand
		{
			get
			{
				return new Command<City>((City) => { 
				Cities.Remove(City); 
				});
			}
		}

		public CitiesViewModel()
		{
			Cities = new ObservableCollection<City>();

			var path = App.FilePath;

			using (var con = new SQLite.SQLiteConnection(App.FilePath))
			{
				con.Table<City>().ToList().ForEach(i => Cities.Add(i));
			}
		}
	}
}
