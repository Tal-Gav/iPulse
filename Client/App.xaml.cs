using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml; 

namespace iPulseApp1
{
    public partial class App : Application
    {
        public static Page BackPage;


        public App()
        {
            InitializeComponent();


            MainPage = new SplashPage();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
