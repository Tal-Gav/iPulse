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
    public partial class ControlPage : ContentPage
    {
        public ControlPage()
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
            await Navigation.PopAsync(true);
        }

        /*
         * The function is responsible to go between help pages.
         * Input: Button, Event
         * Output: None
         */
        private async void GoButton_Clicked(object sender, EventArgs e)
        {
            if (Variables.IsHelpScrren)
            {
                for (var counter = 1; counter < 3; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
                TabbedPage t = ((TabbedPage)(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]));
                t.CurrentPage = t.Children[2];
                await Navigation.PopAsync(true);
            }

            else
                await Navigation.PushAsync(new HeartPage(), true);
        }
    }
}