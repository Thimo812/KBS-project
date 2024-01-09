using KBS_project;
using MatchingAppWindow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MatchingAppWindow.Views
{
    class MatchesDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IsLikedTemplate { get; set; }
        public DataTemplate LikesTemplate { get; set; }
        public DataTemplate MatchTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Match match)
            {
                if (match.Likes && match.IsLiked)
                {
                    return MatchTemplate;
                }
                else
                {
                    if (match.IsLiked)
                    {
                        return IsLikedTemplate;
                    }
                    else
                    {
                        return LikesTemplate;
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
