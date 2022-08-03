using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.SwipeRefreshLayout.Widget;
using SogClientLib;
using SogClientLib.Models;
using SogClientLib.Models.Enums;
using SogClientLib.Models.Interfaces;
using System;
using System.Drawing;
using System.IO;

namespace SogClientAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ViewActivity : AppCompatActivity
    {
        private SogListener _listener;
        private LinearLayout _textLayout;
        private ImageView _imgChords;
        private TextView _txtCaption;
        private TextView _txtText;
        private SwipeRefreshLayout _swipeLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_view);


            _listener = new SogListener();
            _listener.SogMessageRecieved += _listener_SogMessageRecieved;

            try
            {
                SogConnection sogConnection = new SogConnection
                {
                    AppMode = AppMode.Picture,
                    IpAdress = Intent.GetStringExtra("ipAdress"),
                    Port = "8536",
                };
                _listener.ConnectToServerAsync(sogConnection).GetAwaiter().GetResult();
            }

            catch (Exception ex)
            {
                Toast.MakeText(this, $"Some error happen!{System.Environment.NewLine}{ex.Message}", ToastLength.Long).Show();
                _listener.SogMessageRecieved -= _listener_SogMessageRecieved;
                Finish();
            }

            _textLayout = FindViewById<LinearLayout>(Resource.Id.text_layout);
            _imgChords = FindViewById<ImageView>(Resource.Id.img_chords);
            _txtCaption = FindViewById<TextView>(Resource.Id.txt_caption);
            _txtText = FindViewById<TextView>(Resource.Id.txt_text);
            _txtText = FindViewById<TextView>(Resource.Id.txt_text);
            _swipeLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeLayout);
            _swipeLayout.Refresh += _swipeLayout_Refresh;

        }

        private void _swipeLayout_Refresh(object sender, EventArgs e)
        {
            _listener.SwitchMode();
        }

        private void _listener_SogMessageRecieved(ISogMessage obj)
        {
            switch (obj.Type)
            {
                case MessageType.Picture:
                    SetImage(obj.Picture);
                    break;
                case MessageType.Text:
                    SetText(obj.Caption, obj.Text);
                    break;
                default:
                    break;
            }
        }

        private void SetImage(Image picture)
        {
            _imgChords.Visibility = Android.Views.ViewStates.Visible;
            _textLayout.Visibility = Android.Views.ViewStates.Gone;

            byte[] buffer = ImageToByteArray(picture); // read the saved image file 

            Android.Graphics.Bitmap bitmap = BitmapFactory.DecodeByteArray(buffer, 0, buffer.Length);
            _imgChords.SetImageBitmap(bitmap);
        }

        private void SetText(string caption, string text)
        {
            _imgChords.Visibility = Android.Views.ViewStates.Gone;
            _textLayout.Visibility = Android.Views.ViewStates.Visible;

            _txtCaption.Text = caption;
            _txtText.Text = text;
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}