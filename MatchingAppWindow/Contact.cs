using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MatchingAppWindow
{
    public class Contact
    {
        public string UserName { get; set; }
        public BitmapImage ProfileImage { get; set; }

        public Contact(string userName, BitmapImage profileImage)
        {
            UserName = userName;
            ProfileImage = profileImage;
        }
    }
}
