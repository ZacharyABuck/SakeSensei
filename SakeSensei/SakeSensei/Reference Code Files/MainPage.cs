using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Vet
{
    // Lets pages identify themselves.
    public class BaseContentPage : ContentPage
    {
        public string NavagationPageName { get; set; }
    }

    /**
     * The following classes are for the hamburger menu on top of each page.
     * The MasterDetailPage object is defined in the App object.
     **/
    //public class SlidingTrayButton : Button
    //{
    //    public SlidingTrayButton(BaseContentPage page)
    //    {
    //        Text = page.NavagationPageName;
    //        Command = new Command(() =>
    //        {
    //            App.MasterDetailPage.Detail = new NavigationPage(page);
    //            App.MasterDetailPage.IsPresented = false;
    //        });
    //    }
    //}

    //public class MenuPage : ContentPage
    //{
    //    public MenuPage()
    //    {
    //        Content = new StackLayout
    //        {
    //            Padding = new Thickness(0, Device.OnPlatform<int>(20, 0, 0), 0, 0),
    //            Children =
    //            {
    //                new SlidingTrayButton (new ProductListPage(App.SakeDB)),
    //                new SlidingTrayButton (new CustomScanPage())
    //            }
    //        };
    //        Title = "Sliding Tray";
    //        BackgroundColor = Color.Gray.WithLuminosity(0.2);
    //        //Icon = Device.OS == TargetPlatform.iOS ? "slideout.png" : null;

    //    }
    //}

    public class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            var menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = menuPage;
            Detail = new NavigationPage(new PlaceholderPage());
        }

        void NavigateTo(MenuItem menu)
        {
            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);
            Detail = new NavigationPage(displayPage);
            IsPresented = false;
        }
    }

    public class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            Icon = "ic_menu_black_48dp.png";
            Title = "menu"; // The Title property must be set.
            BackgroundColor = Color.FromHex("333333");

            Menu = new MenuListView();

            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    TextColor = Color.FromHex("AAAAAA"),
                    Text = "MENU",
                }
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            Content = layout;
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }

    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<MenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            var cell = new DataTemplate(typeof(ImageCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            ItemTemplate = cell;
        }
    }

    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
            {
                Title = "Product List",
                //IconSource = "contracts.png",
                TargetType = typeof(ProductListPage)
            });

            this.Add(new MenuItem()
            {
                Title = "SakeScanner",
                //IconSource = "Lead.png",
                TargetType = typeof(CustomScanPage)
            });

            //this.Add(new MenuItem()
            //{
            //    Title = "Accounts",
            //    IconSource = "Accounts.png",
            //    TargetType = typeof(AccountsPage)
            //});

            //this.Add(new MenuItem()
            //{
            //    Title = "Opportunities",
            //    IconSource = "Opportunity.png",
            //    TargetType = typeof(OpportunitiesPage)
            //});
        }
    }

    public class PlaceholderPage : ContentPage
    {
        public PlaceholderPage() {
            Title = "Placeholder!";

            var PlaceholderContent = new ContentView
            {
                Content = new Label { Text = "There will be content here!", TextColor = Color.White, },
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(PlaceholderContent);
            Content = layout;
        }
    }
}
