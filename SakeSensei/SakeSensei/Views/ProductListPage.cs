using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Codenutz.XF.Controls;
using Xamarin.Forms;
using Codenutz.XF.InfiniteListView.Shared.Core;
using System.Windows.Input;
using SakeSensei.Models;
using System.Collections.ObjectModel;

namespace SakeSensei.Views
{
    public class BaseContentPage : ContentPage
    {
        public string NavagationPageName { get; set; }
    }


    class ProductListPage : BaseContentPage
    {
        public ProductListPage()
        {
            NavagationPageName = "Product List";

            var searchBar = new SearchBar
            {
                Placeholder = "Search by Name",
                BackgroundColor = Color.White,
                CancelButtonColor = App.BrandColor,
            };

            var sakeListModel = new InfiniteSakeListViewModel(App.SakeDB, Navigation);

            var vetList = new InfiniteListView
            {
                // HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(SakeCell)),
                ItemsSource = sakeListModel.Products,
                BindingContext = sakeListModel,
                LoadMoreCommand = sakeListModel.LoadSakeCommand,
                // SeparatorColor = Color.FromHex("#ddd"),
            };
            vetList.SetBinding(ListView.SelectedItemProperty, "SelectedProduct");

            var layout = new StackLayout
            {
                Children = {
                    searchBar,
                    vetList
                }
            };

            Content = layout;
            Title = "Product List Page";
        }
    }

    public class InfiniteSakeListViewModel : ObservableObject
    {
        public ICommand LoadSakeCommand { get; set; }
        private readonly SakeDB SakeDB;
        private readonly INavigation Navigation;
        public ObservableCollection<Product> _products;
        private Product _selectedProduct;

        public ObservableCollection<Product> Products
        {
            get { return _products ?? (_products = new ObservableCollection<Product>()); }
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                GoToProductPage();
                OnPropertyChanged();
            }
        }

        public InfiniteSakeListViewModel(SakeDB db, INavigation navigation)
        {
            LoadSakeCommand = new Command(GetMoreSake);
            SakeDB = db;
            Navigation = navigation;

            GetMoreSake();
        }

        public async void GetMoreSake()
        {
            var moreProducts = await SakeDB.GetProducts(Products.Count, 15);
            foreach (var product in moreProducts)
            {
                Products.Add(product);
            }
        }

        public void GoToProductPage()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            Navigation.PushModalAsync(new SakeDetailPage(SelectedProduct));
        }
    }

    public class SakeCell : ViewCell
    {
        public SakeCell()
        {
            var vetProfileImage = new ImageCircle.Forms.Plugin.Abstractions.CircleImage
            {
                BorderColor = App.BrandColor,
                BorderThickness = 2,
                HeightRequest = 50,
                WidthRequest = 50,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            // Without an internet connection, the images can't load and the list view locks up.
            // I bet if any of these images become unavailable later, then the list view will lock up.
            // Test
            vetProfileImage.SetBinding(Image.SourceProperty, "ImgS");

            var nameLabel = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 18,
                TextColor = Color.Black
            };
            nameLabel.SetBinding(Label.TextProperty, "SakeName");

            var distanceLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                TextColor = Color.FromHex("#666")
            };
            distanceLabel.SetBinding(Label.TextProperty, "Classification");

            var statusLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { distanceLabel }
            };

            var vetDetailsLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { nameLabel, statusLayout }
            };

            var tapImage = new Image()
            {
                Source = "tap.png",
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 12,
            };

            var cellLayout = new StackLayout
            {
                Spacing = 0,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { vetProfileImage, vetDetailsLayout, tapImage },
            };

            this.View = cellLayout;
        }
    }

    public class SakeDetailPage : ContentPage
    {
        public Product Product { get; set; }

        public SakeDetailPage(Product product)
        {
            this.Product = product;
            Title = "Sake Detail"; // Where is this shown???

            var content = new ContentView
            {
                Content = new Label { Text = Product.SakeName, TextColor = Color.White, },
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(content);
            Content = layout;
        }
    }
}

