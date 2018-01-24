using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;

namespace NumDisplay_Android
{
    [Activity(Label = "NumDisplay", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        private EditText etxIP;

        private Button btnStart;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

            etxIP = FindViewById<EditText>(Resource.Id.etxIP);
            btnStart = FindViewById<Button>(Resource.Id.btnStart);

            btnStart.Click += btnStart_Click;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                MainActivity.address = IPAddress.Parse(etxIP.Text);
                StartActivity(typeof(MainActivity));
            } catch (FormatException)
            {
                Toast.MakeText(this, "Damnit, that's not how IP Addresses work!", ToastLength.Short).Show();
            }
        }
    }
}