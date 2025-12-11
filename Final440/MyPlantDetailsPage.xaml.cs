using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;
using Final440.Styles; 

namespace Final440
{
    public partial class MyPlantDetailsPage : ContentPage
    {
        private readonly MyPlant _myPlant;

        private readonly Label _titleLabel;
        private readonly Editor _instructionsEditor;
        private readonly Editor _notesEditor;
        private readonly Button _saveButton;

        public MyPlantDetailsPage(MyPlant myPlant)
        {
            //InitializeComponent();
            ThemeApp.StylePage(this);

            _myPlant = myPlant;
            Title = myPlant.DisplayName;

            _titleLabel = ThemeApp.CreateTitleLabel($"{myPlant.DisplayName}");
            var subtitle = ThemeApp.CreateBodyLabel($"Base plant: {myPlant.PlantName}");

            _instructionsEditor = new Editor
            {
                Text = myPlant.Instructions,
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 100
            };

            _notesEditor = new Editor
            {
                Text = myPlant.Notes,
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 150
            };

            _saveButton = ThemeApp.CreatePrimaryButton("Save Notes");
            _saveButton.Clicked += async (s, e) => await SaveAsync();

            var instructionsStack = new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    ThemeApp.CreateSectionHeader("Instructions"),
                    _instructionsEditor
                }
            };

            var notesStack = new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    ThemeApp.CreateSectionHeader("Notes / Progress"),
                    _notesEditor
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
                        _titleLabel,
                        subtitle,
                        ThemeApp.WrapInCard(instructionsStack),
                        ThemeApp.WrapInCard(notesStack),
                        _saveButton
                    }
                }
            };
        }

        private async Task SaveAsync()
        {
            _myPlant.Instructions = _instructionsEditor.Text;
            _myPlant.Notes = _notesEditor.Text;

            try
            {
                await DatabaseService.UpdateMyPlantAsync(_myPlant);
                await DisplayAlert("Saved", "Your notes were saved.", "OK");
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Error", "Could not save: " + ex.Message, "OK");
            }
        }
    }
}
