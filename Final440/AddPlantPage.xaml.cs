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
        private readonly Entry _timeToPlantEntry;
        private readonly Entry _amountOfWaterEntry;
        private readonly Entry _amountOfSunlightEntry;
        private readonly Entry _typeOfDirtEntry;
        private readonly Entry _typeOfFoodEntry;
        private readonly Editor _animalsEditor;
        private readonly Editor _allergiesEditor;
        private readonly Entry _toxicEntry;
        private readonly Editor _usesEditor;

        private readonly Button saveButton;

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
                HeightRequest = 80
            };

            _timeToPlantEntry = new Entry
            {
                Placeholder = "Time to plant"
            };

            _amountOfWaterEntry = new Entry
            {
                Placeholder = "Amount of water"
            };

            _amountOfSunlightEntry = new Entry
            {
                Placeholder = "Amount of sunlight"
            };

            _typeOfDirtEntry = new Entry
            {
                Placeholder = "Type of dirt/soil"
            };

            _typeOfFoodEntry = new Entry
            {
                Placeholder = "Type of food/fertilizer"
            };

            _animalsEditor = new Editor
            {
                Placeholder = "Animals attracted/repelled",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 80
            };

            _allergiesEditor = new Editor
            {
                Placeholder = "Allergies",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 80
            };

            _toxicEntry = new Entry
            {
                Placeholder = "Toxic?"
            };

            _usesEditor = new Editor
            {
                Placeholder = "Uses of plant",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 80
            };

            saveButton = new Button
            {
                Text = "Save"
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
                        new Label
                        {
                            Text = "New Plant",
                            FontSize = 24,
                            HorizontalOptions = LayoutOptions.Center
                        },

                        _nameEntry,
                        _descriptionEditor,
                        _timeToPlantEntry,
                        _amountOfWaterEntry,
                        _amountOfSunlightEntry,
                        _typeOfDirtEntry,
                        _typeOfFoodEntry,
                        _animalsEditor,
                        _allergiesEditor,
                        _toxicEntry,
                        _usesEditor,
                        saveButton
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
                Description = _descriptionEditor.Text,
                TimeToPlant = _timeToPlantEntry.Text,
                AmountOfWater = _amountOfWaterEntry.Text,
                AmountOfSunlight = _amountOfSunlightEntry.Text,
                TypeOfDirt = _typeOfDirtEntry.Text,
                TypeOfFood = _typeOfFoodEntry.Text,
                AnimalsAttractedRepelled = _animalsEditor.Text,
                Allergies = _allergiesEditor.Text,
                Toxic = _toxicEntry.Text,
                UsesOfPlant = _usesEditor.Text
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
