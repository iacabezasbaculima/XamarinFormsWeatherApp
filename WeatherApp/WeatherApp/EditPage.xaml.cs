using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeatherApp.ViewModels;

namespace WeatherApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPage : ContentPage
	{
		public EditPage()
		{
			InitializeComponent();
		}
		
		private void btnRemove_Clicked(object sender, EventArgs e)
		{
			var button = sender as ImageButton;
			var city = button.BindingContext as City;
			var vm = BindingContext as CitiesViewModel;

			
			using (var con = new SQLite.SQLiteConnection(App.FilePath))
			{
				int removedCity = con.Delete(city);
				vm.RemoveCommand.Execute(city);
			}
		}
	}
}