using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace NumDisplay_Android
{
    [Activity(Label = "NumDisplay", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        private TextView textView;

        private TcpClient client;
        private StreamReader reader;

        public static IPAddress address;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            textView = FindViewById<TextView>(Resource.Id.textView);

            Connect();
        }

        private void ReadData()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                string data = reader.ReadLine();
                RunOnUiThread(() => textView.Text = data);
            }
        }

        public void Connect()
        {
            new Thread(() =>
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(new IPEndPoint(address, 6000));

                    if (client.Connected)
                    {
                        textView.Text = "Connected!";
                        reader = new StreamReader(client.GetStream());
                        Thread receiveThread = new Thread(() => ReadData());
                        receiveThread.Start();
                    }
                    else
                    {
                        StartActivity(typeof(LoginActivity));
                    }
                }
                catch (SocketException)
                {
                    RunOnUiThread(() => Toast.MakeText(this, "Could not connect, retrying in 5 seconds!", ToastLength.Short).Show());
                    Thread.Sleep(5000);
                    Connect();
                }
            }).Start();
        }
    }
}