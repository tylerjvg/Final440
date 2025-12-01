using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Final440.Services
{
    public class WeatherInfo
    {
        public double Temperature { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public static class WeatherService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string ApiKey = "1954cf626e40fc0b40ec6ab7ac5d46e1"; 
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public static async Task<WeatherInfo?> GetWeatherAsync(string city)
        {
            var url = $"{BaseUrl}?q={Uri.EscapeDataString(city)}&units=imperial&appid={ApiKey}";

            using var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            await using var stream = await response.Content.ReadAsStreamAsync();

            using var doc = await JsonDocument.ParseAsync(stream);
            var root = doc.RootElement;

            var main = root.GetProperty("main");
            var weatherArr = root.GetProperty("weather");

            double temp = main.GetProperty("temp").GetDouble();
            string desc = weatherArr[0].GetProperty("description").GetString() ?? string.Empty;

            return new WeatherInfo
            {
                Temperature = temp,
                Description = desc
            };
        }
    }
}
