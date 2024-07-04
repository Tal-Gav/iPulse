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
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword()
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
            Variables.socket.Send(Encoding.ASCII.GetBytes("BackToLogin"));
            await Navigation.PopAsync(false);
        }

        /*
         * The function is responsible to send check
         * if the email is eligible to change password.
         * Input: Button, Event
         * Output: None
         */
        private async void ResetBTN_Clicked(object sender, EventArgs e)
        {


            if (!string.IsNullOrEmpty(EmailEnt.Text))
            {

                Variables.socket.Send(Encoding.ASCII.GetBytes(EmailEnt.Text));

                byte[] bytes = new byte[256];
                int bytesRec = Variables.socket.Receive(bytes);
                string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                CheckEmail(msg);
            }

            else
                await DisplayAlert("Reset Password Failed", "You must fill all the fields", "OK");
        }

        /*
         * The function is responsible to send check
         * if the email is eligible to change password.
         * Input: msg
         * Output: Task
         */
        private async Task CheckEmail(string msg)
        {
            if (msg == "Email verified")
            {
                await Navigation.PushAsync(new EmailVerificationResetPass(), true);
            }
            else
                await DisplayAlert("Reset Password Failed", msg, "OK");
        }
    }
}