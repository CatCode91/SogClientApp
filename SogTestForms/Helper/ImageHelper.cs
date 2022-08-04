using SogClientLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SogTestForms.Helper
{
    public class ImageHelper
    {
        //questions regarding this logic ask Slava Guk...t.me/slavaguk2000

        private const int countOfPixelPerOneByte = 4;

        public static Bitmap DecodeImage(EncodedImage image)
        {
            Bitmap map = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    int pixelIndex = i * image.Width + j;
                    int tone = ((image.ByteArray[pixelIndex / countOfPixelPerOneByte] >> (3 - pixelIndex % countOfPixelPerOneByte) * 2) & 3) * 255 / 3;
                    map.SetPixel(j, i, Color.FromArgb(tone, tone, tone));
                }
            }
            return map;
        }
    }
}
