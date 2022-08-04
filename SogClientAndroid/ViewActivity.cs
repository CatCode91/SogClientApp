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
using Xamarin.Essentials;
using static Android.Widget.TextView;

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

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_view);

            _textLayout = FindViewById<LinearLayout>(Resource.Id.text_layout);
            _imgChords = FindViewById<ImageView>(Resource.Id.img_chords);
            _txtCaption = FindViewById<TextView>(Resource.Id.txt_caption);
            _txtText = FindViewById<TextView>(Resource.Id.txt_text);
            _txtText = FindViewById<TextView>(Resource.Id.txt_text);


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

                await _listener.ConnectToServerAsync(sogConnection);
            }

            catch (Exception ex)
            {
                Toast.MakeText(this, $"Some error happen!{System.Environment.NewLine}{ex.Message}", ToastLength.Long).Show();
                _listener.SogMessageRecieved -= _listener_SogMessageRecieved;
                Finish();
            }
        }

        private void _listener_SogMessageRecieved(ISogMessage obj)
        {
            switch (obj.Type)
            {
                case MessageType.Picture:
                    SetImage(obj.EncodedImage);
                    break;
                case MessageType.Text:
                    SetText(obj.Caption, obj.Text);
                    break;
                default:
                    break;
            }
        }

        private void SetImage(EncodedImage picture)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _imgChords.Visibility = Android.Views.ViewStates.Visible;
                _textLayout.Visibility = Android.Views.ViewStates.Gone;

                if (picture == null) 
                {
                    return;
                }

                int countOfPixelPerOneByte = 4;
                var bitmap = Android.Graphics.Bitmap.CreateBitmap(picture.Width, picture.Height, Android.Graphics.Bitmap.Config.Argb8888, false);
                for (int i = 0; i < picture.Height; i++)
                {
                    for (int j = 0; j < picture.Width; j++)
                    {
                        int pixelIndex = i * picture.Width + j;
                        int tone = ((picture.ByteArray[pixelIndex / countOfPixelPerOneByte] >> (3 - pixelIndex % countOfPixelPerOneByte) * 2) & 3) * 255 / 3;
                        bitmap.SetPixel(j, i, Android.Graphics.Color.Rgb(tone, tone, tone));
                    }
                }

                _imgChords.SetImageBitmap(bitmap);
            });
        }

        private void SetText(string caption, string text)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _imgChords.Visibility = Android.Views.ViewStates.Gone;
                _textLayout.Visibility = Android.Views.ViewStates.Visible;

                _txtCaption.SetText(caption, BufferType.Normal);
                _txtText.SetText(text, BufferType.Normal);
            });
        }
    }
}