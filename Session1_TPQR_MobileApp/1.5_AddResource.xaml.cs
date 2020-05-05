using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class AddResource : ContentPage
    {
        List<Skill> _skills = new List<Skill>();
        List<Resource_Type> _resourceTypes = new List<Resource_Type>();
        List<int> tempSkills = new List<int>();
        public AddResource()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }

        private void cbCyberSecurity_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbCyberSecurity.IsChecked)
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Cyber Security").Select(x => x.skillId).FirstOrDefault());
            }
            else
            {
                tempSkills.Remove(_skills.Where(x => x.skillName == "Cyber Security").Select(x => x.skillId).FirstOrDefault());
            }
        }


        private void cbWebTech_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbWebTech.IsChecked)
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Web Tech").Select(x => x.skillId).FirstOrDefault());
            }
            else
            {
                tempSkills.Remove(_skills.Where(x => x.skillName == "Web Tech").Select(x => x.skillId).FirstOrDefault());
            }
        }

        private void cbNetworking_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbNetworking.IsChecked)
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Networking").Select(x => x.skillId).FirstOrDefault());
            }
            else
            {
                tempSkills.Remove(_skills.Where(x => x.skillName == "Networking").Select(x => x.skillId).FirstOrDefault());
            }
        }

        private void cbSoftwareSolutions_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbSoftwareSolutions.IsChecked)
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Software Solutions").Select(x => x.skillId).FirstOrDefault());
            }
            else
            {
                tempSkills.Remove(_skills.Where(x => x.skillName == "Software Solutions").Select(x => x.skillId).FirstOrDefault());
            }
        }

        private async void btnAddResource_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Add Resource", "Are you sure you want to add this resource?", "Yes", "No");
            if (userResponse)
            {
                var boolCheck = true;
                using (var webClient = new WebClient())
                {
                    var getType = _resourceTypes.Where(x => x.resTypeName == pickerResourceType.SelectedItem.ToString()).Select(x => x.resTypeId).FirstOrDefault();
                    var newResource = new Resource()
                    {
                        resName = entryResourceName.Text,
                        resTypeIdFK = getType,
                        remainingQuantity = Int32.Parse(entryAvailableQuantity.Text)
                    };
                    var JsonData = JsonConvert.SerializeObject(newResource);
                    webClient.Headers.Add("Content-Type", "application/json");
                    var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resources/Create", "POST", Encoding.UTF8.GetBytes(JsonData));
                    if (Encoding.Default.GetString(response) == "\"Unable to create resource!\"")
                    {
                        boolCheck = false;
                    }
                    else if (Encoding.Default.GetString(response) == "\"Resource Name already exist!\"")
                    {
                        await DisplayAlert("Add Resource", "Resource name is already taken!", "Ok");
                        boolCheck = false;
                    }
                }
                if (boolCheck)
                {
                    using (var webClient = new WebClient())
                    {
                        var getIDResponse = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resources/GetLastestID", "POST", Encoding.UTF8.GetBytes(""));
                        var ResourceID = Convert.ToInt32(JsonConvert.DeserializeObject(Encoding.Default.GetString(getIDResponse)));
                        foreach (var item in tempSkills)
                        {
                            var allocation = new Resource_Allocation() { resIdFK = ResourceID, skillIdFK = item };
                            var JsonData = JsonConvert.SerializeObject(allocation);
                            webClient.Headers.Add("Content-Type", "application/json");
                            var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resource_Allocation/Create", "POST", Encoding.UTF8.GetBytes(JsonData));
                            if (Encoding.Default.GetString(response) == "\"Unable to create allocation!\"")
                            {
                                break;
                            }
                        }

                    }
                }

                if (boolCheck)
                {
                    await DisplayAlert("Add Resource", "Your resource has been added!", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Add Resource", "There was an error while adding your resource! Please contact your administrator!", "Ok");
                }
               

            }
        }

        private void entryAvailableQuantity_Completed(object sender, EventArgs e)
        {
            try
            {
                if (Int32.Parse(entryAvailableQuantity.Text) == 0)
                {
                    cbCyberSecurity.IsEnabled = false;
                    cbSoftwareSolutions.IsEnabled = false;
                    cbNetworking.IsEnabled = false;
                    cbWebTech.IsEnabled = false;
                }
                else
                {
                    cbCyberSecurity.IsEnabled = true;
                    cbSoftwareSolutions.IsEnabled = true;
                    cbNetworking.IsEnabled = true;
                    cbWebTech.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                entryAvailableQuantity.Text = "0";
                cbCyberSecurity.IsEnabled = false;
                cbSoftwareSolutions.IsEnabled = false;
                cbNetworking.IsEnabled = false;
                cbWebTech.IsEnabled = false;
            }
        }

        private async void entryAvailableQuantity_Focused(object sender, FocusEventArgs e)
        {
            await DisplayAlert("Available Quantity", "Please remember to confirm your changes!", "Ok");
        }

        private async void LoadData()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Skills", "POST", Encoding.UTF8.GetBytes(""));
                _skills = JsonConvert.DeserializeObject<List<Skill>>(Encoding.Default.GetString(response));
            }
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resource_Type", "POST", Encoding.UTF8.GetBytes(""));
                _resourceTypes = JsonConvert.DeserializeObject<List<Resource_Type>>(Encoding.Default.GetString(response));
                foreach (var item in _resourceTypes)
                {
                    pickerResourceType.Items.Add(item.resTypeName);
                }
            }
        }
    }
}