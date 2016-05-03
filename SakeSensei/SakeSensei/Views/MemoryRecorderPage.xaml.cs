using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakeSensei.Models;

using Xamarin.Forms;

namespace SakeSensei.Views
{
    public partial class MemoryRecorderPage : ContentPage
    {
        Memory currentMemory;

        public MemoryRecorderPage()
        {
            // Use last saved partial memory form if it exists.
            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey("partialMemory"))
            {
                currentMemory = (Memory)properties["partialMemory"];
            }
            else
            {
                currentMemory = new Memory();
            }

            InitializeComponent();
        }

        public MemoryRecorderPage(Memory seedData)
        {
            // Use last saved partial memory form if it exists.
            IDictionary<string, object> properties = Application.Current.Properties;
            if (properties.ContainsKey("partialMemory"))
            {
                currentMemory = (Memory)properties["partialMemory"];
            }
            else
            {
                currentMemory = new Memory();
            }

            InitializeComponent();

            currentMemory = seedData;
            sakeName.Text = currentMemory.SakeName;
        }

        void OnEntryCompleted(object sender, EventArgs args)
        {
            // This section is going to handle resume!

            //Entry entry = (Entry)sender;

            //currentMemory.SakeName = sakeName.Text;
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Diagnostic tool; will be a toast pop-up, but not there yet.
            await DisplayAlert("Memory", "Sake Name: " + currentMemory.SakeName, "ok");

            // Return to whence ye came!
            await Navigation.PopAsync();
        }

        public void OnSleep()
        {
            // Save current form fill
            Application.Current.Properties["partialMemory"] = currentMemory;
        }
    }
}

