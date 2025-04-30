using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClockV2.Alarm
{
    /// <summary>
    /// Represents an alarm time with various properties such as display time, date, name, description, trigger time, and unique identifier.
    /// </summary>
    public class AlarmTime
    {
        private string DisplayTime { get; }
        private DateTime Date { get; }
        private string Name { get; set; }
        private string Description { get; set; }
        private TimeSpan TriggerTime { get; set; }
        private String Uid { get; set; }
        private DateTime SetDate { get; }

        /// <summary>
        /// Constructor for the AlarmTime class.
        /// </summary>
        /// <param name="displayTime">The text to be dislpayed beneath the clock</param>
        /// <param name="date">The time of the alarm</param>
        /// <param name="triggerTime">(FOR EXPORT) The reminder time</param>
        /// <param name="name">The alarm's name</param>
        /// <param name="description">The alarm's description</param>
        /// <param name="uid">The unique identifer for the alarm</param>
        /// <param name="setDate">The time the alarm was made</param>
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

        /// <summary>
        /// Override the ToString method to return a string representation of the alarm.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ": " + DisplayTime;
        }

        /// <summary>
        /// Gets the display time string of the alarm.
        /// </summary>
        /// <returns></returns>
        public string GetDT()
        {
            return DisplayTime;
        }

        /// <summary>
        /// Gets the name of the alarm.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Gets the Description of the alarm.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return Description;
        }

        /// <summary>
        /// Gets the date of the alarm.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDate()
        {
            return Date;
        }

        /// <summary>
        /// Export the alarm to a calendar event format.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Override the Equals method to compare two AlarmTime objects.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var other = (AlarmTime)obj;
            return DisplayTime == other.DisplayTime && Date == other.Date;
        }

        /// <summary>
        /// Override the GetHashCode method to provide a hash code for the AlarmTime object.
        /// </summary>
        /// <returns></returns>
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
