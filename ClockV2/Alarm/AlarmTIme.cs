using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClockV2.Alarm
{
    public class AlarmTime
    {
        public string DisplayTime { get; }
        public DateTime Date { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan TriggerTime { get; set; }

        public AlarmTime(string displayTime, DateTime date, TimeSpan triggerTime, string name, string description)
        {
            DisplayTime = displayTime;
            Date = date;
            Name = name;
            Description = description;
            TriggerTime = triggerTime;
        }

        public override string ToString()
        {
            return DisplayTime;
        }

        public DateTime GetDate()
        {
            return Date;
        }

        public string ToCalanderEvent()
        {
            StringBuilder ical = new StringBuilder();
            ical.AppendLine("BEGIN:VEVENT");
            ical.AppendLine($"SUMMARY:{Name}");
            ical.AppendLine($"DESCRIPTION:{Description}");
            ical.AppendLine($"DTSTART:{Date:yyyyMMddTHHmmssZ}");
            ical.AppendLine($"DTSTAMP:{DateTime.Now:yyyyMMddTHHmmssZ}");
            ical.AppendLine("BEGIN:VALARM");
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
