using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SakeSensei.Views
{
    public partial class MemoryListPage : ContentPage
    {
        public MemoryListPage()
        {
            InitializeComponent();
        }
    }

    async void OnRecordMemoryButtonClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new MemoryRecorderPage(new Models.Memory("SO SAKE")));
    }
}
