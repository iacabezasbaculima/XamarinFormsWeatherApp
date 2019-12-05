using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
	public class City
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public City()
		{

		}
	}
}
