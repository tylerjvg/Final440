using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;

namespace Final440
{
    public partial class MyPlantsPage : ContentPage
    {
        private readonly CollectionView collectionView;
        private readonly Button addButton;

        private List<MyPlant> _myPlants = new();

        public MyPlantsPage()
        {
            Title = "My Plant Feed";

            addButton = new Button
            {
                Text = "Add Plant to My Feed"
            };
            addButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AddMyPlantPage());
            };

            collectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nameLabel = new Label
                    {
                        FontSize = 18,
                        VerticalOptions = LayoutOptions.Center
                    };
                    nameLabel.SetBinding(Label.TextProperty, "DisplayName");

                    var plantNameLabel = new Label
                    {
                        FontSize = 14,
                        TextColor = Colors.Gray
                    };
                    plantNameLabel.SetBinding(Label.TextProperty, "PlantName");

                    return new VerticalStackLayout
                    {
                        Padding = new Thickness(10, 5),
                        Children = { nameLabel, plantNameLabel }
                    };
                })
            };

            collectionView.SelectionChanged += OnSelectionChanged;

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            var headerLayout = new StackLayout
            {
                Padding = 10,
                Children = { addButton }
            };

            grid.Children.Add(headerLayout);
            grid.Children.Add(collectionView);

            Grid.SetRow(headerLayout, 0);
            Grid.SetRow(collectionView, 1);

            Content = grid;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadMyPlantsAsync();
        }

        private async Task LoadMyPlantsAsync()
        {
            try
            {
                _myPlants = await DatabaseService.GetMyPlantsAsync();
                collectionView.ItemsSource = _myPlants;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load feed: " + ex.Message, "OK");
            }
        }

        private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            var selected = e.CurrentSelection[0] as MyPlant;
            if (selected != null)
            {
                await Navigation.PushAsync(new MyPlantDetailsPage(selected));
            }

            collectionView.SelectedItem = null;
        }
    }
}
