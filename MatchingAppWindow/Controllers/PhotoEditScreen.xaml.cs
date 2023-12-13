using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for PhotoEditScreen.xaml
    /// </summary>
    public partial class PhotoEditScreen : Page
    {
        public ObservableCollection<BitmapImage> Images { get; set; }

        public PhotoEditScreen()
        {
            InitializeComponent();

            DataContext = this;

            Images = new();
        }

        public void InitializePage()
        {
            foreach (byte[] imageData in MainWindow.profile.Images)
            {
                Images.Add(ImageConverter.ImageDataToBitmap(imageData));
            }
        }
    }
}
