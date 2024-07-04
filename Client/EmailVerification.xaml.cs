using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iPulseApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailVerification : ContentPage
    {
        public EmailVerification()
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
            Variables.socket.Send(Encoding.ASCII.GetBytes("BackToSignUp,"));
            await Navigation.PopAsync(false);
        }

        /*
         * The function is responsible to send the verification code
         * to the server and gets a validation to the code from the server.
         * Input: Button, Event
         * Output: None
         */
        private void ConfirmBTN_Clicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes(CodeEnt.Text));

            byte[] bytes = new byte[256];
            int bytesRec = Variables.socket.Receive(bytes);
            string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            CheckAccount(msg);
        }

         /*
         * The function is responsible to check if the account can sign up
         * depends on the server response.
         * Input: msg
         * Output: Task
         */
        public async Task CheckAccount(string msg)
        {
            if (msg == "Account Created")
            {
                Variables.IsHelpScrren = false;
                await Navigation.PushAsync(new HeyPage(), true);
            }
            else
                await DisplayAlert("Signup Failed", msg, "OK");
        }
    }
}