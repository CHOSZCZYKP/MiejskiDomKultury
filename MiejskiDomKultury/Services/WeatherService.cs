using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace MiejskiDomKultury.Services
{
  
    public class WeatherService
    {
   
        int lat;
        int lon;

        
        public async Task GetLocationAsync()
        {
            using HttpClient client = new HttpClient();
           
            string response = await client.GetStringAsync("http://ip-api.com/json/");
            JObject json = JObject.Parse(response);

            
            double latitude = (double)json["lat"];
            double longitude = (double)json["lon"];

            Console.WriteLine($"Latitude: {latitude}, Longitude: {longitude}");
            
            this.lat = (int)latitude;
            this.lon = (int)longitude;
        }

        public async Task<string> GetWeather()
        {
       
            await GetLocationAsync();

       
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={this.lat}&lon={this.lon}&appid={Environment.GetEnvironmentVariable("WEATHER_API")}";

            using HttpClient client = new HttpClient();
          
            string response = await client.GetStringAsync(url);
            JObject data = JObject.Parse(response);

          
            double temp = (double)data["main"]["temp"];  
            double windSpeed = (double)data["wind"]["speed"];  

            temp = temp - 273.15;  
            windSpeed = windSpeed * 3.6;  

         
            return $"Temperatura {temp.ToString("F1")}°C\nWiatr {windSpeed.ToString("F1")} km/h";
        }
    }
}
