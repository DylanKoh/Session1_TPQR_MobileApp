using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
        List<Resource_Type> _resourceTypes = new List<Resource_Type>();
        List<Skill> _skill = new List<Skill>();
        public ResourceManagement()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadResources();
            LoadPickers();

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

        private async void LoadPickers()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resource_Type", "POST", Encoding.UTF8.GetBytes(""));
                _resourceTypes = JsonConvert.DeserializeObject<List<Resource_Type>>(Encoding.Default.GetString(response));
                foreach (var item in _resourceTypes)
                {
                    pickerType.Items.Add(item.resTypeName);
                }
            }
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Skills", "POST", Encoding.UTF8.GetBytes(""));
                _skill = JsonConvert.DeserializeObject<List<Skill>>(Encoding.Default.GetString(response));
                foreach (var item in _skill)
                {
                    pickerSkill.Items.Add(item.skillName);
                }
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

        private void pickerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerSkill.SelectedItem == null)
            {
                var filteredList = (from x in _customViews
                                    where x.ResourceType == pickerType.SelectedItem.ToString()
                                    select x);
                lvResources.ItemsSource = filteredList.ToList();
            }
            else
            {
                var filteredList = (from x in _customViews
                                    where x.ResourceType == pickerType.SelectedItem.ToString() && x.AllocatedSkills.Contains(pickerSkill.SelectedItem.ToString())
                                    select x);
                lvResources.ItemsSource = filteredList.ToList();
            }
        }

        private void pickerSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerType.SelectedItem == null)
            {
                var filteredList = (from x in _customViews
                                    where x.AllocatedSkills.Contains(pickerSkill.SelectedItem.ToString())
                                    select x);
                lvResources.ItemsSource = filteredList.ToList();
            }
            else
            {
                var filteredList = (from x in _customViews
                                    where x.ResourceType == pickerType.SelectedItem.ToString() && x.AllocatedSkills.Contains(pickerSkill.SelectedItem.ToString())
                                    select x);
                lvResources.ItemsSource = filteredList.ToList();
            }
        }
    }
}