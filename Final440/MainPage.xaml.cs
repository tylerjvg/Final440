using Final440.Services;
using Microsoft.Maui.Controls;
using Final440.Models;
using System;
using System.Threading.Tasks;

namespace Final440
{
    public partial class MainPage : ContentPage
    {
        private readonly Label weatherLabel;
        private readonly Label plantOfDayLabel;
        private readonly Image plantOfDayImage;
        private readonly Button plantsButton;
        private readonly Button addPlantButton;
        private readonly Button myPlantsButton;


        public MainPage()
        {
            InitializeComponent();

            Title = "Home";

            weatherLabel = new Label
            {
                Text = "Loading weather",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center
            };

            plantOfDayLabel = new Label
            {
                Text = "Loading Plant of the Day",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center
            };

            plantOfDayImage = new Image
            {
                HeightRequest = 120,
                WidthRequest = 120,
                HorizontalOptions = LayoutOptions.Center
            };

            plantsButton = new Button
            {
                Text = "View Plants"
            };
            plantsButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new PlantsPage());
            };

            addPlantButton = new Button
            {
                Text = "Add New Plant"
            };
            addPlantButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AddPlantPage());
            };
            myPlantsButton = new Button
            {
                Text = "My Plant Feed"
            };
            myPlantsButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new MyPlantsPage());
            };


            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 20,
                    Children =
                    {
                        new Label
                        {
                            Text = "Plant App",
                            FontSize = 26,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        weatherLabel,
                        plantOfDayLabel,
                        plantOfDayImage,
                        plantsButton,
                        addPlantButton,
                        myPlantsButton

                    }
                }
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadWeatherAsync();
            await LoadPlantOfTheDayAsync();
        }

        private async Task LoadWeatherAsync()
        {
            try
            {
                var info = await WeatherService.GetWeatherAsync("Chicago");
                if (info == null)
                {
                    weatherLabel.Text = "Weather unavailable.";
                    return;
                }

                weatherLabel.Text = $"Weather: {info.Temperature:F1}°F, {info.Description}";
            }
            catch (Exception ex)
            {
                weatherLabel.Text = "Weather error: " + ex.Message;
            }
        }

        private async Task LoadPlantOfTheDayAsync()
        {
            try
            {
                var allPlants = await DatabaseService.GetAllPlantsAsync();
                var result = DatabaseService.PickPlantOfTheDay(allPlants, DateTime.Now);

                if (result.Plant == null)
                {
                    plantOfDayLabel.Text = "No plants available.";
                    plantOfDayImage.Source = "placeholder.png";
                    return;
                }

                if (result.IsSeasonal)
                {
                    plantOfDayLabel.Text = $"Plant of the day: {result.Plant.Name}";
                }
                else
                {
                    plantOfDayLabel.Text = $"Nothing to plant today, so here’s a cool plant: {result.Plant.Name}";
                }

                plantOfDayImage.Source = result.Plant.ImageSource;
            }
            catch (Exception ex)
            {
                plantOfDayLabel.Text = $"Plant of the day error: {ex.Message}";
                plantOfDayImage.Source = "placeholder.png";
            }
        }


    }
}
