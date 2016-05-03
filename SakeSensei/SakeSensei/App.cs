﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net.Interop;

using Xamarin.Forms;
using SakeSensei.Views;
using SakeSensei.Models;

namespace SakeSensei
{
    public class App : Application
    {
        public static Color BrandColor = Color.FromHex("#4E64D1");
        public static MasterDetailPage MasterDetailPage;
        public static SakeDB SakeDB { get; private set; }

        public App(String dbPath, ISQLitePlatform sqlitePlatform)
        {
            SakeDB = new SakeDB(sqlitePlatform, dbPath);


            //var page = new RootPage();

            //page.ToolbarItems.Add (new ToolbarItem () { Icon = "settings.png" });

            //var nav = new NavigationPage (page) {
            //	BarTextColor = Color.White,
            //	BarBackgroundColor = Color.FromHex ("#4E64D1"),
            //};

            MainPage = new MainTabbedPage();

        }

        protected override void OnResume()
        {
            // I'M GONNA FILL THIS IN NEXT
            // GOTTA RESUME, MAN
            // GOTTA RESUME
        }

        protected override void OnSleep()
        {
            // For future use.
        }

        protected override void OnStart()
        {
            // For future use.
        }
    }
}
