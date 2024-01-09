using KBS_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MatchingAppWindow.Views
{
    class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SenderTemplate { get; set; }
        public DataTemplate ReceiverTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Message message)
            {
                return message.IsSender ? SenderTemplate : ReceiverTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
