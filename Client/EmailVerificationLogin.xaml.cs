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
    public partial class EmailVerificationLogin : ContentPage
    {
        public EmailVerificationLogin()
        {
            InitializeComponent();
        }

        /*
         * The function is responsible to take to a previous page.
         * Input: Button, Event
         * Output: None
         */
        async private void BackButton_Clicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("BackToLogin,"));
            await Navigation.PopAsync(false);
        }

        /*
         * The function is responsible to send the verification code
         * to the server and gets a validation to the code from the server.
         * Input: Button, Event
         * Output: None
         */
        private async void ConfirmBTN_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CodeEnt.Text))
            {
                Variables.socket.Send(Encoding.ASCII.GetBytes(CodeEnt.Text));
                byte[] bytes = new byte[256];
                int bytesRec = Variables.socket.Receive(bytes);
                string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                CheckAccount(msg);
            }
            else
                await DisplayAlert("Login Failed", "Wrong code", "OK");
        }

        /*
         * The function is responsible to check if the account can log in
         * depends on the server response.
         * Input: msg
         * Output: Task
         */
        public async Task CheckAccount(string msg)
        {
            if (msg == "Account Verified")
            {
                await Navigation.PushAsync(new HeartPage(), true);
            }

            else
                await DisplayAlert("Login Failed", msg, "OK");
        }
    }
}