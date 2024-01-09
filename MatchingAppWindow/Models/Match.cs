using MatchingAppWindow.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MatchingAppWindow.Model
{
    public class Match
    {
        public string UserName { get; set; }
        public bool Likes { get; set; }
        public bool IsLiked { get; set; }


        public Match(string userName, bool likes, bool isLiked)
        {
            UserName = userName;
            Likes = likes;
            IsLiked = isLiked;
        }
    }
}
