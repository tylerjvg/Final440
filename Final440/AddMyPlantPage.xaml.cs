using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;
using Final440.Styles;

namespace Final440
{
    public partial class AddMyPlantPage : ContentPage
    {
        private readonly Picker _plantPicker;
        private readonly Entry _customNameEntry;
        private readonly Editor _instructionsEditor;
        private readonly Editor _notesEditor;
        private readonly Button _saveButton;

        private List<Plant> _allPlants = new();

        public AddMyPlantPage()
        {
            ThemeApp.StylePage(this);
            Title = "Add to My Feed";

            var titleLabel = ThemeApp.CreateTitleLabel("Add Plant to my Feed");
            var subtitleLabel = ThemeApp.CreateBodyLabel("Choose a plant");

            _plantPicker = new Picker
            {
                Title = "Select plant"
            };

            _customNameEntry = new Entry
            {
                Placeholder = "Custom name"
            };

            _instructionsEditor = new Editor
            {
                Placeholder = "Your instructions ",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 80
            };

            _notesEditor = new Editor
            {
                Placeholder = "Initial notes",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 80
            };

            _saveButton = ThemeApp.CreatePrimaryButton("Save to My Feed");
            _saveButton.Clicked += async (s, e) => await SaveAsync();

            var formStack = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    ThemeApp.CreateSectionHeader("Plant Selection"),
                    _plantPicker,
                    _customNameEntry,

                    ThemeApp.CreateSectionHeader("Care & Notes"),
                    _instructionsEditor,
                    _notesEditor,

                    _saveButton
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
                        subtitleLabel,
                        ThemeApp.WrapInCard(formStack)
                    }
                }
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPlantsAsync();
        }

        private async Task LoadPlantsAsync()
        {
            _allPlants = await DatabaseService.GetAllPlantsAsync();
            _plantPicker.ItemsSource = _allPlants.Select(p => p.Name).ToList();
        }

        private async Task SaveAsync()
        {
            if (_plantPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Validation", "Please select a plant.", "OK");
                return;
            }

            var plant = _allPlants[_plantPicker.SelectedIndex];

            var myPlant = new MyPlant
            {
                PlantID = plant.PlantID,
                PlantName = plant.Name,
                CustomName = string.IsNullOrWhiteSpace(_customNameEntry.Text)
                    ? null
                    : _customNameEntry.Text.Trim(),
                Instructions = _instructionsEditor.Text,
                Notes = _notesEditor.Text
            };

            try
            {
                await DatabaseService.AddMyPlantAsync(myPlant);
                await DisplayAlert("Success", "Plant added to your feed.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Could not add plant: " + ex.Message, "OK");
            }
        }
    }
}
