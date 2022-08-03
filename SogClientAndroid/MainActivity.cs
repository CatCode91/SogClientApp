using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.Snackbar;
using SogClientLib;
using SogClientLib.Models;
using System;

namespace SogClientAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button _btnConnect;
        private SogListener _listener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _listener = new SogListener();

           _btnConnect = FindViewById<Button>(Resource.Id.btn_connect);
            _btnConnect.Click += _btnConnect_Click;
           // SetSupportActionBar(toolbar);

            //loatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
           // fab.Click += FabOnClick;
        }

        private void _btnConnect_Click(object sender, EventArgs e)
        {
            SogConnection sogConnection = new SogConnection
            {
                AppMode = SogClientLib.Models.Enums.AppMode.Text,
                IpAdress = "192.168.1.177",
                Port = "8536",
            };

            Intent intent = new Intent(this, typeof(ViewActivity));
            intent.PutExtra("ipAdress", sogConnection.IpAdress);
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
