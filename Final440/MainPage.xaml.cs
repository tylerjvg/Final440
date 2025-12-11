using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;
using Final440.Styles;

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
            ThemeApp.StylePage(this);
            Title = "Home";
            var header = ThemeApp.CreateTitleLabel("Plant App");

            weatherLabel = ThemeApp.CreateBodyLabel("Loading weather...");

            plantOfDayLabel = ThemeApp.CreateBodyLabel("Loading Plant of the Day");

            plantOfDayImage = new Image
            {
                HeightRequest = 120,
                WidthRequest = 120,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 8)
            };

            plantsButton = ThemeApp.CreatePrimaryButton("Browse All Plants");
            plantsButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new PlantsPage());
            };

            addPlantButton = ThemeApp.CreateSecondaryButton("Add New Plant");
            addPlantButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AddPlantPage());
            };
            myPlantsButton = ThemeApp.CreateSecondaryButton("My Plant Feed");
            myPlantsButton.BackgroundColor = ThemeApp.AccentBlue;
            myPlantsButton.TextColor = Colors.White;
            myPlantsButton.BorderWidth = 0;
            myPlantsButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new MyPlantsPage());
            };
            var weatherCard = ThemeApp.WrapInCard(
                            new VerticalStackLayout
                            {
                                Spacing = 4,
                                Children =
                                {
                        ThemeApp.CreateSectionHeader("Today’s Weather"),
                        weatherLabel
                                }
                            });

            var plantCard = ThemeApp.WrapInCard(
                new VerticalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        ThemeApp.CreateSectionHeader("Plant of the Day"),
                        plantOfDayLabel,
                        plantOfDayImage
                    }
                });

            var buttonsCard = ThemeApp.WrapInCard(
                new VerticalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        plantsButton,
                        addPlantButton,
                        myPlantsButton
                    }
                });

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = new Thickness(20, 20),
                    Spacing = 12,
                    Children =
                    {
                        weatherCard,
                        plantCard,
                        buttonsCard
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
