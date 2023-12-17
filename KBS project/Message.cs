using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBS_project
{
    public class Message
    {
        public DateTime TimeStamp { get; set; }
        public string Content { get; set; }
        public bool IsSender { get; set; }

        public Message(DateTime timeStamp, string content, bool isSender)
        {
            TimeStamp = timeStamp;
            Content = content;
            IsSender = isSender;
        }

        public string DateString => ReturnDateString();

        private string ReturnDateString()
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(TimeStamp, localTimeZone);

            if (localTime.Date.Equals(DateTime.Today))
            {
                return $"Today at {localTime.ToString("HH:mm")}";
            }
            else if (localTime.Date.Equals(DateTime.Today.AddDays(-1)))
            {
                return $"Yesterday at {localTime.ToString("HH:mm")}";
            }

            return localTime.ToString("yyyy-MM-dd");
        }
    }
}
