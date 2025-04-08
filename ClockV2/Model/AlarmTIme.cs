using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClockV2.Alarm
{
    public class AlarmTime
    {
        private string DisplayTime { get; }
        private DateTime Date { get; }
        private string Name { get; set; }
        private string Description { get; set; }
        private TimeSpan TriggerTime { get; set; }
        private String Uid { get; set; }
        private DateTime SetDate { get; }

        public AlarmTime(string displayTime, DateTime date, TimeSpan triggerTime, string name, string description, string uid, object setDate)
        {
            DisplayTime = displayTime;
            Date = date;
            Name = name;
            Description = description;
            TriggerTime = triggerTime;
            Uid = uid;
            if (setDate != null)
            {
                SetDate = (DateTime)setDate;

            }

        }

        public override string ToString()
        {
            return Name + ": " + DisplayTime;
        }

        public string GetDT()
        {
            return DisplayTime;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDescription()
        {
            return Description;
        }

        public DateTime GetDate()
        {
            return Date;
        }

        public string ToCalanderEvent()
        {
            StringBuilder ical = new StringBuilder();
            ical.AppendLine("BEGIN:VEVENT");
            if (Uid != "")
            {
                ical.AppendLine($"UID:{Uid}");
            }
            else
            {
                ical.AppendLine($"UID:{Guid.NewGuid()}");

            }
            ical.AppendLine($"SUMMARY:{Name}");
            ical.AppendLine($"DESCRIPTION:{Description}");
            ical.AppendLine($"DTSTART:{Date.ToUniversalTime():yyyyMMddTHHmmssZ}");
            if (SetDate != DateTime.MinValue)
            {
                ical.AppendLine($"DTSTAMP:{SetDate.ToUniversalTime():yyyyMMddTHHmmssZ}");
            }
            else
            {
                ical.AppendLine($"DTSTAMP:{DateTime.Now.ToUniversalTime():yyyyMMddTHHmmssZ}");
            }
            ical.AppendLine("BEGIN:VALARM");
            ical.AppendLine("ACTION:DISPLAY");
            ical.AppendLine($"TRIGGER:{(TriggerTime < TimeSpan.Zero ? "-" : "+")}{Math.Abs(TriggerTime.TotalMinutes)}M");
            ical.AppendLine($"DESCRIPTION:{Description} Alarm Trigger");
            ical.AppendLine($"SUMMARY:{Name} Alarm Trigger");
            ical.AppendLine("END:VALARM");
            ical.AppendLine("END:VEVENT");
            return ical.ToString();

        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var other = (AlarmTime)obj;
            return DisplayTime == other.DisplayTime && Date == other.Date;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = 17;
                hash = hash * 23 + (DisplayTime != null ? DisplayTime.GetHashCode() : 0);
                hash = hash * 23 + Date.GetHashCode();
                return hash;
            }
        }
    }
}
