using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;

namespace Final440
{
    public partial class PlantsPage : ContentPage
    {
        private readonly CollectionView collectionView;
        private List<Plant> _plants = new();

        public PlantsPage()
        {
            Title = "Plants";

            collectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemTemplate = new DataTemplate(() =>
                {
                    var image = new Image
                    {
                        HeightRequest = 40,
                        WidthRequest = 40,
                        VerticalOptions = LayoutOptions.Center
                    };
                    image.SetBinding(Image.SourceProperty, "ImageSource");

                    var nameLabel = new Label
                    {
                        FontSize = 18,
                        VerticalOptions = LayoutOptions.Center
                    };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    var grid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = 50 },
                            new ColumnDefinition { Width = GridLength.Star }
                        },
                        Padding = new Thickness(10, 5)
                    };

                    grid.Children.Add(image);
                    grid.Children.Add(nameLabel);

                    Grid.SetColumn(image, 0);
                    Grid.SetColumn(nameLabel, 1);

                    return grid;
                })
            };

            collectionView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
            collectionView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical);

            collectionView.SelectionChanged += OnSelectionChanged;

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 10,
                    Children =
                    {
                        collectionView
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
            try
            {
                _plants = await DatabaseService.GetAllPlantsAsync();
                collectionView.ItemsSource = _plants;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load plants: " + ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            var selectedPlant = e.CurrentSelection[0] as Plant;
            if (selectedPlant != null)
            {
                await Navigation.PushAsync(new PlantDetailsPage(selectedPlant));
            }

            collectionView.SelectedItem = null;
        }
    }
}
