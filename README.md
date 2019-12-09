# WeatherX
A weather app developed in C# .NET Framework with Xamarin.Forms for Android devices.

# The App
VIsual Studio 2019 is used as the main development environment, WeatherX is officially untested in other IDEs.

# OpenWeatherMap API
The weather data is fetched from [openweathermap.org](https://openweathermap.org/).
 
To access OpenWeatherMap API:

* A OpenWeatherMap [App Key](https://openweathermap.org/appid) is required (WeatherX uses 5 Day / 3 Hour forecast data)

# Using Weather Data
WeatherX executes OpenWeatherMap API calls to fetch data in XML format, [example](https://samples.openweathermap.org/data/2.5/forecast?q=London,us&mode=xml&appid=b6907d289e10d714a6e88b30761fae22).  

# Google Places API
The [Place Autocomplete](https://developers.google.com/places/web-service/autocomplete) feature of Google's Places API is used to predict the user's preferred city.

As the user inputs every other character we fetch the predictions with an API call, for example:

`https://maps.googleapis.com/maps/api/place/autocomplete/json?input=<SEARCH_TEXT>&types=geocode&key=<API_KEY>`

[`<SEARCH_TEXT>`] is the input from the user and [`<API_KEY>`](https://developers.google.com/places/web-service/get-api-key) is the user-specific key which is required for each API call.

# Using Place Autocomplete Data
The API call returns JSON formatted data of five distinct places, for example:

```json
{
  "status": "OK",
  "predictions" : [
      {
         "description" : "Paris, France",
         "distance_meters" : 8030004,
         "id" : "691b237b0322f28988f3ce03e321ff72a12167fd",
         "matched_substrings" : [
            {
               "length" : 5,
               "offset" : 0
            }
         ],
         "place_id" : "ChIJD7fiBh9u5kcRYJSMaMOCCwQ",
         "reference" : "CjQlAAAA_KB6EEceSTfkteSSF6U0pvumHCoLUboRcDlAH05N1pZJLmOQbYmboEi0SwXBSoI2EhAhj249tFDCVh4R-PXZkPK8GhTBmp_6_lWljaf1joVs1SH2ttB_tw",
         "terms" : [
            {
               "offset" : 0,
               "value" : "Paris"
            },
            {
               "offset" : 7,
               "value" : "France"
            }
         ],
         "types" : [ "locality", "political", "geocode" ]
      },
      {
        "Additional predictions...": 4
      }]
}
```
For more information, please visit [Places API Docs](https://developers.google.com/places/web-service/intro).


