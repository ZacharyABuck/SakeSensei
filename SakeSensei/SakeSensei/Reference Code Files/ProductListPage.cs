using Codenutz.XF.Controls;
using Codenutz.XF.InfiniteListView.Shared.Core;
using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Codenutz.XF.InfiniteListView.Shared.Core
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

namespace Codenutz.XF.Controls
{
    /// <summary>
    /// A simple listview that exposes a bindable command to allow infinite loading behaviour.
    /// </summary>
    public class InfiniteListView : ListView
    {
        /// <summary>
        /// Respresents the command that is fired to ask the view model to load additional data bound collection.
        /// </summary>
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<InfiniteListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));

        /// <summary>
        /// Gets or sets the command binding that is called whenever the listview is getting near the bottomn of the list, and therefore requiress more data to be loaded.
        /// </summary>
        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="InfiniteListView" />
        /// </summary>
        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }


        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }


    }
}

namespace Vet
{
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
                Children = { distanceLabel}
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
