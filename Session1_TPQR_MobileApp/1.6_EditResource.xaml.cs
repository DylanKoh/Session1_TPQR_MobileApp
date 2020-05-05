using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class EditResource : ContentPage
    {
        string _resourceName;
        List<Resource_Allocation> _resourceAllocation = new List<Resource_Allocation>();
        List<Skill> _skills = new List<Skill>();
        Resource _resource = new Resource();
        List<int> tempSkills = new List<int>();
        List<Resource_Type> _resourceType = new List<Resource_Type>();
        public EditResource(string ResourceName)
        {
            InitializeComponent();
            _resourceName = ResourceName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }

        private async void LoadData()
        {
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:63881/Resources/Details?ResourceName={_resourceName}", "POST", Encoding.UTF8.GetBytes(""));
                _resource = JsonConvert.DeserializeObject<Resource>(Encoding.Default.GetString(response));
                lblResourceName.Text = _resource.resName;
                entryAvailableQuantity.Text = _resource.remainingQuantity.ToString();
            }
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Skills", "POST", Encoding.UTF8.GetBytes(""));
                _skills = JsonConvert.DeserializeObject<List<Skill>>(Encoding.Default.GetString(response));
            }
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:63881/Resource_Allocation/GetDetails?resID={_resource.resId}", "POST", Encoding.UTF8.GetBytes(""));
                _resourceAllocation = JsonConvert.DeserializeObject<List<Resource_Allocation>>(Encoding.Default.GetString(response));
                foreach (var item in _resourceAllocation)
                {
                    var skill = _skills.Where(x => x.skillId == item.skillIdFK).Select(x => x.skillName).FirstOrDefault();
                    switch (skill)
                    {
                        case "Cyber Security":
                            cbCyberSecurity.IsChecked = true;
                            break;
                        case "Networking":
                            cbNetworking.IsChecked = true;
                            break;
                        case "Web Tech":
                            cbWebTech.IsChecked = true;
                            break;
                        case "Software Solutions":
                            cbSoftwareSolutions.IsChecked = true;
                            break;
                        default:
                            break;
                    }
                    tempSkills.Add(_skills.Where(x => x.skillId == item.skillIdFK).Select(x => x.skillId).FirstOrDefault());
                }
            }
            using (var webClient = new WebClient())
            {
                var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:63881/Resource_Type", "POST", Encoding.UTF8.GetBytes(""));
                _resourceType = JsonConvert.DeserializeObject<List<Resource_Type>>(Encoding.Default.GetString(response));
                var resourceType = _resourceType.Where(x => x.resTypeId == _resource.resTypeIdFK).Select(x => x.resTypeName).FirstOrDefault();
                lblResourceType.Text = resourceType;
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

        private void cbCyberSecurity_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbCyberSecurity.IsChecked && !tempSkills.Contains(_skills.Where(x => x.skillName == "Cyber Security").Select(x => x.skillId).FirstOrDefault()))
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Cyber Security").Select(x => x.skillId).FirstOrDefault());
            }
            else if (!cbCyberSecurity.IsChecked)
            {
                var id = _skills.Where(x => x.skillName == "Cyber Security").Select(x => x.skillId).FirstOrDefault();
                var todelete = tempSkills.Where(x => x == id).Select(x => x).ToList();
                foreach (var item in todelete)
                {
                    tempSkills.Remove(item);
                }
            }
        }

        private void cbSoftwareSolutions_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbSoftwareSolutions.IsChecked && !tempSkills.Contains(_skills.Where(x => x.skillName == "Software Solutions").Select(x => x.skillId).FirstOrDefault()))
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Software Solutions").Select(x => x.skillId).FirstOrDefault());
            }
            else if (!cbSoftwareSolutions.IsChecked)
            {
                var id = _skills.Where(x => x.skillName == "Software Solutions").Select(x => x.skillId).FirstOrDefault();
                var todelete = tempSkills.Where(x => x == id).Select(x => x).ToList();
                foreach (var item in todelete)
                {
                    tempSkills.Remove(item);
                }
            }
        }

        private void cbNetworking_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbNetworking.IsChecked && !tempSkills.Contains(_skills.Where(x => x.skillName == "Networking").Select(x => x.skillId).FirstOrDefault()))
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Networking").Select(x => x.skillId).FirstOrDefault());
            }
            else if (!cbNetworking.IsChecked)
            {
                var id = _skills.Where(x => x.skillName == "Networking").Select(x => x.skillId).FirstOrDefault();
                var todelete = tempSkills.Where(x => x == id).Select(x => x).ToList();
                foreach (var item in todelete)
                {
                    tempSkills.Remove(item);
                }
            }
        }

        private void cbWebTech_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (cbWebTech.IsChecked && !tempSkills.Contains(_skills.Where(x => x.skillName == "Web Tech").Select(x => x.skillId).FirstOrDefault()))
            {
                tempSkills.Add(_skills.Where(x => x.skillName == "Web Tech").Select(x => x.skillId).FirstOrDefault());
            }
            else if (!cbWebTech.IsChecked)
            {
                var id = _skills.Where(x => x.skillName == "Web Tech").Select(x => x.skillId).FirstOrDefault();
                var todelete = tempSkills.Where(x => x == id).Select(x => x).ToList();
                foreach (var item in todelete)
                {
                    tempSkills.Remove(item);
                }
            }
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Edit Resource", "Are you sure you want to add this resource?", "Yes", "No");
            if (userResponse)
            {
                var boolCheck = true;
                using (var webClient = new WebClient())
                {
                    _resource.remainingQuantity = Int32.Parse(entryAvailableQuantity.Text);
                    var JsonData = JsonConvert.SerializeObject(_resource);
                    webClient.Headers.Add("Content-Type", "application/json");
                    var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Resources/Edit", "POST", Encoding.UTF8.GetBytes(JsonData));
                    if (Encoding.Default.GetString(response) == "\"Unable to edit resource!\"")
                    {
                        boolCheck = false;
                    }
                }
                if (boolCheck)
                {
                    using (var webClient = new WebClient())
                    {
                        await webClient.UploadDataTaskAsync($"http://10.0.2.2:63881/Resource_Allocation/Delete?ResID={_resource.resId}", "POST", Encoding.UTF8.GetBytes(""));
                        foreach (var item in tempSkills.Distinct())
                        {
                            var allocation = new Resource_Allocation() { resIdFK = _resource.resId, skillIdFK = item };
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
                    await DisplayAlert("Edit Resource", "Your resource has been edited successfully!", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Edit Resource", "There was an error while editing your resource! Please contact your administrator!", "Ok");
                }


            }
        }
    }
}