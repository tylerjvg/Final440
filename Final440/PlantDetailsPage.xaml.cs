using Microsoft.Maui.Controls;
using Final440.Models;

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

            var img = new Image
            {
                Source = plant.ImageSource,
                HeightRequest = 150,
                HorizontalOptions = LayoutOptions.Center
            };

            var layout = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 8,
                Children =
                {
                    img,
                    CreateLabel("Description: " + plant.Description),
                    CreateLabel("Time to plant: " + plant.TimeToPlant),
                    CreateLabel("Water: " + plant.AmountOfWater),
                    CreateLabel("Sunlight: " + plant.AmountOfSunlight),
                    CreateLabel("Soil: " + plant.TypeOfDirt),
                    CreateLabel("Food: " + plant.TypeOfFood),
                    CreateLabel("Animals: " + plant.AnimalsAttractedRepelled),
                    CreateLabel("Allergies: " + plant.Allergies),
                    CreateLabel("Toxic: " + plant.Toxic),
                    CreateLabel("Uses: " + plant.UsesOfPlant)
                }
            };

            Content = new ScrollView { Content = layout };
        }

        private static Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                FontSize = 16
            };
        }
    }
}
