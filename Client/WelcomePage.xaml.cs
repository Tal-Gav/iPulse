using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using System.Threading;

namespace iPulseApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            ConnectToServerAsync();
        }

        /*
         * The function is responsible to go to the sign up page.
         * Input: Button, Event
         * Output: None
         */
        private async void SignUpBTN_Clicked(object sender, EventArgs e)
        {
            App.BackPage = new SignUpPage();
            await Navigation.PushAsync(App.BackPage,false);
            Variables.socket.Send(Encoding.ASCII.GetBytes("Signup"));

        }

        /*
         * The function is responsible to go to the log in page.
         * Input: Button, Event
         * Output: None
         */
        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            App.BackPage = new LogInPage();
            await Navigation.PushAsync(App.BackPage, false);
            Variables.socket.Send(Encoding.ASCII.GetBytes("Login"));
        }

        /*
         * The function is responsible to connect to the server.
         * Input: None
         * Output: Task
         */
        public async Task ConnectToServerAsync() // connect to the server
        {
            Variables.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("Local IP"), 8090);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("Host IP"), 8090);

            while (true)
            {
                try
                {
                    Variables.socket.Connect(ipPoint);
                    break;
                }
                catch (Exception e)
                {
                    await DisplayAlert("Alert", "Cannot connect", "OK");
                }
            }

            Variables.socket.Send(Encoding.ASCII.GetBytes("Connected"));
            byte[] bytes = new byte[256];
            int bytesRec = Variables.socket.Receive(bytes);
            string msg = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            await DisplayAlert("Alert", msg, "OK");

        }

    }
}