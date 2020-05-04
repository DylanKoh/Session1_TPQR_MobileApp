using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Session1_TPQR_MobileApp.GlobalClass;

namespace Session1_TPQR_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResourceManagement : ContentPage
    {
        List<CustomView> _customViews = new List<CustomView>();
        public ResourceManagement()
        {
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            LoadResources();
        }

        private async void LoadResources()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resources", "POST", Encoding.UTF8.GetBytes(""));
                _customViews = JsonConvert.DeserializeObject<List<CustomView>>(Encoding.Default.GetString(response));
                lvResources.ItemsSource = _customViews;
            }
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {

        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {

        }

        private void btnRemove_Clicked(object sender, EventArgs e)
        {

        }
    }
}