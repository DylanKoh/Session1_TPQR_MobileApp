using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Session1_TPQR_MobileApp.GlobalClass;

namespace Session1_TPQR_MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            entryPassword.Text = string.Empty;
            entryUserID.Text = string.Empty;
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (entryPassword.Text.Trim() == string.Empty || entryUserID.Text.Trim() == string.Empty)
            {
                await DisplayAlert("Login", "Your field(s) are empty! Please check them and try again later!", "Ok");
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Content-Type", "application/json");
                    var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:63881/Users/Login?username={entryUserID.Text}&password={entryPassword.Text}", "POST",
                        Encoding.UTF8.GetBytes(""));
                    if (Encoding.Default.GetString(response) != "\"User does not exist. Please try again!\"")
                    {
                        var user = JsonConvert.DeserializeObject<User>(Encoding.Default.GetString(response));
                        if (user.userTypeIdFK == 1)
                        {
                            await DisplayAlert("Login", $"Welcome {user.userName}!", "Ok");
                            await Navigation.PushAsync(new ResourceManagement());
                        }
                        else
                        {
                            await DisplayAlert("Login", $"Welcome {user.userName}! Unfortunately, your account is not permitted to access this application!", "Ok");
                            entryPassword.Text = string.Empty;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Login", "User does not exist.Please try again!", "Ok");
                        entryPassword.Text = string.Empty;
                    }
                }
            }
        }
    }
}