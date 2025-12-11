using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Final440.Models;
using Final440.Services;
using Final440.Styles; 

namespace Final440
{
    public partial class MyPlantsPage : ContentPage
    {
        private readonly CollectionView _collectionView;
        private readonly Button _addButton;

        private List<MyPlant> _myPlants = new();

        public MyPlantsPage()
        {
            //InitializeComponent();
            ThemeApp.StylePage(this);
            Title = "My Plant Feed";

            var titleLabel = ThemeApp.CreateTitleLabel("My Plant Feed");
            var subtitleLabel = ThemeApp.CreateBodyLabel("Track the plants you owm");

            _addButton = ThemeApp.CreatePrimaryButton("Add Plant for my Feed");
            _addButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AddMyPlantPage());
            };

            _collectionView = new CollectionView
            {
                SelectionMode = SelectionMode.None, 
                ItemTemplate = new DataTemplate(() =>
                {
                    var nameLabel = new Label
                    {
                        FontSize = 18,
                        TextColor = ThemeApp.TextPrimary,
                        VerticalOptions = LayoutOptions.Center
                    };
                    nameLabel.SetBinding(Label.TextProperty, "DisplayName");

                    var plantNameLabel = new Label
                    {
                        FontSize = 14,
                        TextColor = ThemeApp.TextSecondary
                    };
                    plantNameLabel.SetBinding(Label.TextProperty, "PlantName");

                    var innerStack = new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children = { nameLabel, plantNameLabel }
                    };

                    var frame = new Frame
                    {
                        BackgroundColor = ThemeApp.CardBackground,
                        CornerRadius = 12,
                        HasShadow = false,
                        BorderColor = ThemeApp.BorderSoft,
                        Padding = new Thickness(10, 6),
                        Margin = new Thickness(10, 4, 10, 4),
                        Content = innerStack
                    };

                    var tap = new TapGestureRecognizer();
                    tap.Tapped += async (s, e) =>
                    {
                        if (frame.BindingContext is MyPlant myPlant)
                        {
                            await Application.Current.MainPage.Navigation
                                .PushAsync(new MyPlantDetailsPage(myPlant));
                        }
                    };

                    frame.GestureRecognizers.Add(tap);

                    return frame;
                })
            };

            var headerLayout = new VerticalStackLayout
            {
                Padding = new Thickness(20, 20, 20, 10),
                Spacing = 6,
                Children =
                {
                    titleLabel,
                    subtitleLabel,
                    _addButton
                }
            };

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            grid.Children.Add(headerLayout);
            grid.Children.Add(_collectionView);

            Grid.SetRow(headerLayout, 0);
            Grid.SetRow(_collectionView, 1);

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
                _collectionView.ItemsSource = _myPlants;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load feed: " + ex.Message, "OK");
            }
        }
    }
}
