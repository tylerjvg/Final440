using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Styles;

namespace Final440
{
    public partial class PlantDetailsPage : ContentPage
    {
        private readonly Plant _plant;

        public PlantDetailsPage(Plant plant)
        {
            //InitializeComponent();

            _plant = plant;
            Title = plant.Name;
            var titleLabel = ThemeApp.CreateTitleLabel(_plant.Name);


            var img = new Image
            {
                Source = _plant.ImageSource,
                HeightRequest = 160,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 8)
            };

            var infoStack = new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    ThemeApp.CreateSectionHeader("Overview"),
                    ThemeApp.CreateBoldPrefixLabel("Description: ", _plant.Description),
                    ThemeApp.CreateBoldPrefixLabel("Time to plant: ", _plant.TimeToPlant),
                    ThemeApp.CreateSectionHeader("Care"),
                    ThemeApp.CreateBoldPrefixLabel("Water: ",  _plant.AmountOfWater),
                    ThemeApp.CreateBoldPrefixLabel("Sunlight: ",  _plant.AmountOfSunlight),
                    ThemeApp.CreateBoldPrefixLabel("Soil: ",  _plant.TypeOfDirt),
                    ThemeApp.CreateBoldPrefixLabel("Food: ",  _plant.TypeOfFood),
                    ThemeApp.CreateSectionHeader("Other"),
                    ThemeApp.CreateBoldPrefixLabel("Animals: ",  _plant.AnimalsAttractedRepelled),
                    ThemeApp.CreateBoldPrefixLabel("Allergies: ",  _plant.Allergies),
                    ThemeApp.CreateBoldPrefixLabel("Toxic: ",  _plant.Toxic),
                    ThemeApp.CreateBoldPrefixLabel("Uses: ",  _plant.UsesOfPlant)
                }
            };


            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 10,
                    Children =
                    {
                        titleLabel,
                        ThemeApp.WrapInCard(img),
                        ThemeApp.WrapInCard(infoStack)
                    }
                }
            };
        }
    }
}
