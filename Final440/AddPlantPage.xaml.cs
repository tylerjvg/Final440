using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;

namespace Final440
{
    public partial class AddPlantPage : ContentPage
    {
        private readonly Entry _nameEntry;
        private readonly Editor _descriptionEditor;
        private readonly Button _saveButton;

        public AddPlantPage()
        {
            //InitializeComponent();

            Title = "Add Plant";

            _nameEntry = new Entry
            {
                Placeholder = "Name"
            };

            _descriptionEditor = new Editor
            {
                Placeholder = "Description",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 100
            };

            _saveButton = new Button
            {
                Text = "Save"
            };
            _saveButton.Clicked += async (s, e) => await SaveAsync();

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 12,
                    Children =
                    {
                        new Label
                        {
                            Text = "New Plant",
                            FontSize = 24,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        _nameEntry,
                        _descriptionEditor,
                        _saveButton
                    }
                }
            };
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                await DisplayAlert("Validation", "Name is required.", "OK");
                return;
            }

            var plant = new Plant
            {
                Name = _nameEntry.Text.Trim(),
                Description = _descriptionEditor.Text
            };

            try
            {
                await DatabaseService.AddPlantAsync(plant);
                await DisplayAlert("Success", "Plant added.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not save plant: {ex.Message}", "OK");
            }
        }
    }
}
