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
            pickerSkill.Items.Clear();
            pickerType.Items.Clear();
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

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddResource());
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            var selected = (CustomView)lvResources.SelectedItem;
            Console.WriteLine(selected.ResourceName);
            await Navigation.PushAsync(new EditResource(selected.ResourceName));
        }

        private async void btnRemove_Clicked(object sender, EventArgs e)
        {
            var selected = (CustomView)lvResources.SelectedItem;
            var userResponse = await DisplayAlert("Remove", $"Are you sure you want to remove {selected.ResourceName}?", "Yes", "No");
            if (userResponse)
            {
                using(var webClient = new WebClient())
                {
                    try
                    {
                        webClient.Headers.Add("Content-Type", "application/json");
                        var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:63881/Resources/Delete?ResourceName={selected.ResourceName}", "POST",
                            Encoding.UTF8.GetBytes(""));
                        if (Encoding.Default.GetString(response) == "\"Resource has been successfully deleted!\"")
                        {
                            await DisplayAlert("Remove", "Resource has been successfully deleted!", "Ok");
                            LoadResources();
                        }
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Remove", "An error occured while trying to remove resource. Please try again later!", "Ok");
                    }
                    
                }
            }
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