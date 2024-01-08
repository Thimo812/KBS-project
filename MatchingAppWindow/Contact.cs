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
        public bool IsIncomingRequest { get; set; }
        public bool IsOutgoingRequest { get; set; }

        public Contact(string userName, BitmapImage profileImage)
        {
            UserName = userName;
            ProfileImage = profileImage;
            IsIncomingRequest = false;
            IsOutgoingRequest = false;
        }

        public Contact(string userName, BitmapImage profileImage, bool isIncRequest, bool isOutRequest)
        {
            UserName = userName;
            ProfileImage = profileImage;
            IsIncomingRequest = isIncRequest;
            IsOutgoingRequest = isOutRequest;
        }
    }
}
