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
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        /*
         * The function is responsible to show the Splash Page when opening the app.
         * Input: None
         * Output: None
         */
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(async () => {

                await Task.Delay(3000);

                 Device.BeginInvokeOnMainThread(() => { App.Current.MainPage = new NavigationPage(new WelcomePage()); }); // WelcomePage
                 // Device.BeginInvokeOnMainThread(() => { App.Current.MainPage = new NavigationPage(new HeartPage()); }); // HeartPage
            });

        }
    }
}