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
    public partial class ChangePassword : ContentPage
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        /*
         * The function is responsible to take a page back.
         * Input: Button, Event
         * Output: None
         */
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("BackToResetPass"));

            for (var counter = 1; counter < 2; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count-2]);
            }
            await Navigation.PopAsync(false);
        }

        /*
         * The function is responsible to take to the reset password page.
         * Input: Button, Event
         * Output: None
         */
        private async void ResetBTN_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(PassEnt.Text))
            {
                Variables.socket.Send(Encoding.ASCII.GetBytes(PassEnt.Text));

                byte[] bytes = new byte[256];
                int bytesRec = Variables.socket.Receive(bytes);
                string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                if (msg == "Password Updated")
                {
                    await DisplayAlert("Password Reset Successfully ", "", "OK");

                    for (var counter = 1; counter < 3; counter++)
                    {
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                    await Navigation.PopAsync(false);
                }
            }
            else
                await DisplayAlert("Reset Password Failed", "You must fill all the fields", "OK");
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
    }

}