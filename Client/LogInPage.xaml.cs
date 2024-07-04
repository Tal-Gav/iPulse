using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iPulseApp1
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();

        }

        /*
         * The function is responsible to take to a previous page.
         * Input: Button, Event
         * Output: None
         */
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("BackToMain"));
            await Navigation.PopToRootAsync(false);
        }

        /*
         * The function is responsible to go to the sign up page.
         * Input: Button, Event
         * Output: None
         */
        private async void SendToSignUp_Clicked(object sender, EventArgs e)
        {
            if (App.BackPage is SignUpPage)
            {
                Variables.socket.Send(Encoding.ASCII.GetBytes("ToSignUp"));
                await Navigation.PopAsync(false);
            }
            else
            {
                Variables.socket.Send(Encoding.ASCII.GetBytes("ToSignUp"));
                await Navigation.PushAsync(new SignUpPage(), false);
            }
        }

        /*
         * The function is responsible to show/hide the password.
         * Input: Button, Event
         * Output: None
         */
        private void PassVisible_Clicked(object sender, EventArgs e)
        {
          
            if (PassEnt.IsPassword == true)
            {
                PassEnt.IsPassword = false;
                eyePink.Opacity = 1;
                eyeGrey.Opacity = 0;
            }
            else
            {
                PassEnt.IsPassword = true;
                eyePink.Opacity = 0;
                eyeGrey.Opacity = 1;
            }

        }

        /*
         * The function is responsible to go to the reset password page.
         * Input: Button, Event
         * Output: None
         */
        private async void ResetBTN_Clicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("ResetPass"));
            await Navigation.PushAsync(new ResetPassword(), false);

        }

        /*
         * The function is responsible to send the login details to the server.
         * Input: Button, Event
         * Output: None
         */
        private async void LoginBTN_Clicked(object sender, EventArgs e)
            
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("Login," + EmailEnt.Text + "," + PassEnt.Text));

            byte[] bytes = new byte[256];
            int bytesRec = Variables.socket.Receive(bytes);
            string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            CheckIfAccountExist(msg);
        }

        /*
         * The function is responsible to check if the account
         * is valid to login depends on the server's response.
         * Input: Button, Event
         * Output: None
         */
        public async Task CheckIfAccountExist(string msg)
        {
            if (msg == "CanLogin")
            {
                await Navigation.PushAsync(new EmailVerificationLogin(), true);
            }
            else if (msg == "You must fill all the fields.")
                await DisplayAlert(msg, "", "OK");
            else
                await DisplayAlert(msg, "Mail or password don't match.", "OK");
        }
    }
}