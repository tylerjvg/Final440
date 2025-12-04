using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;

namespace Final440
{
    public partial class AddMyPlantPage : ContentPage
    {
        private readonly Picker plantPicker;
        private readonly Entry customNameEntry;
        private readonly Editor instructionsEditor;
        private readonly Editor notesEditor;
        private readonly Button saveButton;

        private List<Plant> _allPlants = new();

        public AddMyPlantPage()
        {
            Title = "Add to My Feed";

            plantPicker = new Picker
            {
                Title = "Select plant"
            };

            customNameEntry = new Entry
            {
                Placeholder = "Custom name (optional)"
            };

            instructionsEditor = new Editor
            {
                Placeholder = "Your instructions",
                AutoSize = EditorAutoSizeOption.TextChanges,
                HeightRequest = 80
            };

            notesEditor = new Editor
            {
                Placeholder = "Initial notes",
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
                            Text = "Add Plant to My Feed",
                            FontSize = 24,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        plantPicker,
                        customNameEntry,
                        instructionsEditor,
                        notesEditor,
                        saveButton
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
            plantPicker.ItemsSource = _allPlants.Select(p => p.Name).ToList();
        }

        private async Task SaveAsync()
        {
            if (plantPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Validation", "Please select a plant.", "OK");
                return;
            }

            var plant = _allPlants[plantPicker.SelectedIndex];

            var myPlant = new MyPlant
            {
                PlantID = plant.PlantID,
                PlantName = plant.Name,
                CustomName = string.IsNullOrWhiteSpace(customNameEntry.Text)
                    ? null
                    : customNameEntry.Text.Trim(),
                Instructions = instructionsEditor.Text,
                Notes = notesEditor.Text
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
