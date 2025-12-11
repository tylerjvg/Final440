using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;
using Final440.Styles;

namespace Final440
{
    public partial class PlantsPage : ContentPage
    {
        private readonly CollectionView collectionView;
        private List<Plant> _plants = new();

        public PlantsPage()
        {
            //InitializeComponent();
            ThemeApp.StylePage(this);
            Title = "Plants";

            var header = ThemeApp.CreateTitleLabel("All Plants");
            var sub = ThemeApp.CreateBodyLabel("Browse everything in the database");

            collectionView = new CollectionView
            {
                SelectionMode = SelectionMode.None,
                ItemTemplate = new DataTemplate(() =>
                {
                    var image = new Image
                    {
                        HeightRequest = 40,
                        WidthRequest = 40,
                        VerticalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFill
                    };
                    image.SetBinding(Image.SourceProperty, "ImageSource");

                    var nameLabel = new Label
                    {
                        FontSize = 18,
                        TextColor = ThemeApp.TextPrimary,
                        VerticalOptions = LayoutOptions.Center
                    };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    var rowLayout = new Grid
                    {
                        ColumnDefinitions =
            {
                new ColumnDefinition { Width = 55 },
                new ColumnDefinition { Width = GridLength.Star }
            },
                        ColumnSpacing = 12
                    };

                    rowLayout.Children.Add(image);
                    rowLayout.Children.Add(nameLabel);
                    Grid.SetColumn(image, 0);
                    Grid.SetColumn(nameLabel, 1);

                    var frame = new Frame
                    {
                        BackgroundColor = ThemeApp.CardBackground,
                        CornerRadius = 12,
                        HasShadow = false,
                        BorderColor = ThemeApp.BorderSoft,
                        Padding = new Thickness(10, 6),
                        Margin = new Thickness(10, 4, 10, 4),
                        Content = rowLayout
                    };

                    var tap = new TapGestureRecognizer();
                    tap.Tapped += async (s, e) =>
                    {
                        if (frame.BindingContext is Plant plant)
                        {
                            await Application.Current.MainPage.Navigation
                                .PushAsync(new PlantDetailsPage(plant));
                        }
                    };

                    frame.GestureRecognizers.Add(tap);

                    return frame;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            collectionView.SelectionChanged += OnSelectionChanged;

            var listCard = ThemeApp.WrapInCard(collectionView);

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 8,
                    Children =
                    {
                        header,
                        sub,
                        listCard
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
