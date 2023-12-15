using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MatchingAppWindow
{
    public static class ImageConverter
    {
        public static BitmapImage ImageDataToBitmap(byte[] imageData)
        {
            if (imageData == null) return null;
            BitmapImage bitmapImage = new();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        public static byte[] ImagePathToData(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static byte[] BitmapImageToData(BitmapImage bitmapImage)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                encoder.Save(stream);

                byte[] imageData = stream.ToArray();

                return imageData;
            }
        }
    }
}
