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
    }
}
