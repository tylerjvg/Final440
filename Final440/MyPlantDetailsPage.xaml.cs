using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;

namespace Final440
{
    public partial class MyPlantDetailsPage : ContentPage
    {
        private readonly MyPlant _myPlant;

        private readonly Label titleLabel;
        private readonly Editor instructionsEditor;
        private readonly Editor notesEditor;
        private readonly Button saveButton;

        public MyPlantDetailsPage(MyPlant myPlant)
        {
            _myPlant = myPlant;

            Title = myPlant.DisplayName;

            titleLabel = new Label
            {
                Text = $"{myPlant.DisplayName} ({myPlant.PlantName})",
                FontSize = 22,
                HorizontalOptions = LayoutOptions.Center
            };

            instructionsEditor = new Editor
            {
                Text = myPlant.Instructions,
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 100
            };

            notesEditor = new Editor
            {
                Text = myPlant.Notes,
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 150
            };

            saveButton = new Button
            {
                Text = "Save Notes"
            };
            saveButton.Clicked += async (s, e) => await SaveAsync();

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 12,
                    Children =
                    {
                        titleLabel,
                        new Label { Text = "Instructions:" },
                        instructionsEditor,
                        new Label { Text = "Notes / Progress:" },
                        notesEditor,
                        saveButton
                    }
                }
            };
        }

        private async Task SaveAsync()
        {
            _myPlant.Instructions = instructionsEditor.Text;
            _myPlant.Notes = notesEditor.Text;

            try
            {
                await DatabaseService.UpdateMyPlantAsync(_myPlant);
                await DisplayAlert("Saved", "Your notes were saved.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not save: " + ex.Message, "OK");
            }
        }
    }
}
