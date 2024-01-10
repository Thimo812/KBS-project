using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Windows.Media.Imaging;
using MatchingAppWindow.Core;

namespace MatchingAppTest
{
   

    [TestClass]
    public class ImageconverterTest
    {
        [TestMethod]
        public void ImageDataToBitmap_NullImageData_ReturnsNull()
        {
            byte[] imageData = null;

            var result = ImageConverter.ImageDataToBitmap(imageData);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ImageDataToBitmap_ValidImageData_ReturnsBitmapImage()
        {
            byte[] imageData = File.ReadAllBytes("../../../../MatchingAppWindow/Images/AccountIcon.png");

            var result = ImageConverter.ImageDataToBitmap(imageData);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BitmapImage));
        }

        [TestMethod]
        public void ImagePathToData_ValidPath_ReturnsImageData()
        {
            string imagePath = "../../../../MatchingAppWindow/Images/AccountIcon.png";

            var result = ImageConverter.ImagePathToData(imagePath);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void BitmapImageToData_ValidBitmapImage_ReturnsImageData()
        {
            byte[] imageData = File.ReadAllBytes("../../../../MatchingAppWindow/Images/AccountIcon.png");
            BitmapImage bitmapImage = ImageConverter.ImageDataToBitmap(imageData);

            var result = ImageConverter.BitmapImageToData(bitmapImage);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}
