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
    public partial class Create : ContentPage
    {
        List<User_Type> _userTypes = new List<User_Type>();
        public Create()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPicker();
        }


        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            if (entryPassword.Text == entryRePassword.Text)
            {
                var getUserTypeID = (from x in _userTypes
                                     where x.userTypeName == pickerUserType.SelectedItem.ToString()
                                     select x.userTypeId).FirstOrDefault();
                var newUser = new User()
                {
                    userId = entryUserID.Text,
                    userName = entryUserName.Text,
                    userPw = entryPassword.Text,
                    userTypeIdFK = getUserTypeID
                };
                using (var webClient = new WebClient())
                {
                    var jsonData = JsonConvert.SerializeObject(newUser);
                    webClient.Headers.Add("Content-Type", "application/json");
                    var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/Users/Create", "POST", Encoding.UTF8.GetBytes(jsonData));
                    if (Encoding.Default.GetString(response) == "\"User has been created!\"")
                    {
                        await DisplayAlert("Create Account", "Your account has been created successfully!", "Ok");
                        await Navigation.PopAsync();
                    }
                    else if (Encoding.Default.GetString(response) == "\"User ID has been taken!\"")
                    {
                        await DisplayAlert("Create Account", "Your desired User ID has been taken! Please choose another User ID!", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Create Account", "Your account has not been created! Please check your fields and try again!", "Ok");
                    }
                }

            }
            else
            {
                await DisplayAlert("Create Account", "Your passwords do not match!", "Ok");
            }
            
        }

        private async void LoadPicker()
        {
            using(var webClient = new WebClient())
            {
                webClient.Headers.Add("Contect-type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:63881/User_Type", "POST", Encoding.UTF8.GetBytes(""));
                _userTypes = JsonConvert.DeserializeObject<List<User_Type>>(Encoding.Default.GetString(response));
                foreach (var item in _userTypes)
                {
                    pickerUserType.Items.Add(item.userTypeName);
                }
            }
        }
    }
}