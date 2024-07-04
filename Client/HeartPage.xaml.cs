using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iPulseApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeartPage : TabbedPage
    {
        IPEndPoint ipPoint;
        public HeartPage()
        {
            InitializeComponent();

            this.CurrentPage = Children[2];
            Variables.msrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipPoint = new IPEndPoint(IPAddress.Parse("ESP32 IP"), 80);
        }  

        /*
         * The function is responsible to measure the pulse
         * from the connected board and show it to the client.
         * Input: Button, Event
         * Output: None
         */
        private async void ButtonMsr_Clicked(object sender, EventArgs e)
        {

            if (Variables.msrSocket.Connected)
            {
                ((Button)sender).IsEnabled = false;

                HeartStart.Source = "HeartGIF_Final2.gif";
                HeartStart.IsAnimationPlaying = true;

                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    Variables.msrSocket.Send(Encoding.ASCII.GetBytes("GetBpm"));

                    byte[] bytes = new byte[256];
                    int bytesRec = Variables.msrSocket.Receive(bytes);
                    string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                // do something every 60 seconds
                Device.BeginInvokeOnMainThread(() =>
                    {
                        BPM.Text = msg;
                    });

                    return true; // runs again, or false to stop
                });
            }

            else
                await DisplayAlert("Alert", "You have to connect the board before measuring BPM.", "OK");
        }

        /*
         * The function is responsible to connect to the ESP32 board.
         * Input: Button, Event
         * Output: None
         */
        private async void ConnectToBoardBTN_Clicked(object sender, EventArgs e)
        {
                ((Button)sender).IsEnabled = false;
                Variables.msrSocket.Connect(ipPoint);
                await DisplayAlert("Alert", "Connected to the board.", "OK");
        }

        /*
         * The function is responsible to go to the help page.
         * Input: Button, Event
         * Output: None
         */
        async private void ToHelp_Clicked(object sender, EventArgs e)
        {
            Variables.IsHelpScrren = true;
            await Navigation.PushAsync(new HeyPage(), true);
        }

        /*
         * The function is responsible to log out and go to the main page.
         * Input: Button, Event
         * Output: None
         */
        private async void ToLogout_CLicked(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("BackToSelection"));
            await Navigation.PopToRootAsync(true);
        }

        /*
         * The function is responsible to go to the account page.
         * Input: Button, Event
         * Output: None
         */
        private void Account(object sender, EventArgs e)
        {
            Variables.socket.Send(Encoding.ASCII.GetBytes("AccountScreen"));

             byte[] bytes = new byte[256];
             int bytesRec = Variables.socket.Receive(bytes);
             string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);
             ShowAccount(msg);
        }

        /*
         * The function is responsible to show the account details.
         * Input: Button, Event
         * Output: None
         */
        public void ShowAccount(string msg)
        {
            List<string> account = msg.Split(',').ToList();
            
            AccountEnt.Placeholder = account[1] + " " + account[2];
            EmailEnt.Placeholder = account[0];
        }

    }
}