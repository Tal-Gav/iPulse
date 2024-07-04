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
    public partial class InEmergencyPage : ContentPage
    {
        public InEmergencyPage()
        {
            InitializeComponent();
        }

        /*
         * The function is responsible to go between help pages.
         * Input: Button, Event
         * Output: None
         */
        private async void GoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ControlPage(), true);
        }

        /*
         * The function is responsible to take to a previous page.
         * Input: Button, Event
         * Output: None
         */
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
    }
 
}