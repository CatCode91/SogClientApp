using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SogClientAndroid.Helpers
{
    public class ImageHelper
    {
        //questions regarding this logic ask Slava Guk...t.me/slavaguk2000

        private const int countOfPixelPerOneByte = 4;

        public static Bitmap DecodeImage(int height, int width, byte[] encodedArray)
        {
            Bitmap map = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int pixelIndex = i * width + j;
                    int tone = ((encodedArray[pixelIndex / countOfPixelPerOneByte] >> (3 - pixelIndex % countOfPixelPerOneByte) * 2) & 3) * 255 / 3;
                    map.SetPixel(j, i, Color.FromArgb(tone, tone, tone));
                }
            }
            return map;
        }
    }
}