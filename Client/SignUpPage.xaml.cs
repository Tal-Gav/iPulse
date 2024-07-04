using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Net.Sockets;

namespace iPulseApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {

        public SignUpPage()
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
         * The function is responsible to go to the log in page.
         * Input: Button, Event
         * Output: None
         */
        private async void SendToLogIn_Clicked(object sender, EventArgs e)
        {
            if (App.BackPage is LogInPage)
            {
                Variables.socket.Send(Encoding.ASCII.GetBytes("ToLogIn"));
                await Navigation.PopAsync(false);
            }
            else
            {
                Variables.socket.Send(Encoding.ASCII.GetBytes("ToLogIn"));
                await Navigation.PushAsync(new LogInPage(), false);
            }
        }

        /*
         * The function is responsible to show/hide the password.
         * Input: Button, Event
         * Output: None
         */
        private void PassVisible_Clicked(object sender, EventArgs e)
        {
            if (PasswordEnt.IsPassword == true)
            {
                PasswordEnt.IsPassword = false;
                eyePink.Opacity = 1;
                eyeGrey.Opacity = 0;
            }
            else
            {
                PasswordEnt.IsPassword = true;
                eyePink.Opacity = 0;
                eyeGrey.Opacity = 1;
            }
        }

        /*
         * The function is responsible to send the signup details to the server.
         * Input: Button, Event
         * Output: None
         */
        async void SignUpBTN_Clicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("SignUp," + FirstNameEnt.Text + "," + LastNameEnt.Text + "," + EmailEnt.Text + "," + PasswordEnt.Text));

            byte[] bytes = new byte[256];
            int bytesRec = Variables.socket.Receive(bytes);
            string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            CheckIfAccountExist(msg);
        }

        /*
         * The function is responsible to check if the account
         * is valid to signup depends on the server's response.
         * Input: Button, Event
         * Output: None
         */
        public async Task CheckIfAccountExist(string msg)
        {
            if (msg == "CanSignUp")
                await Navigation.PushAsync(new EmailVerification(), true);
            else
                await DisplayAlert("Signup Failed", msg, "OK");
        }


    }
}