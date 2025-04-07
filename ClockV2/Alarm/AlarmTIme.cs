using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockV2.Alarm
{
    public class AlarmTime
    {
        public string DisplayTime { get; }
        public DateTime Date { get; }

        public AlarmTime(string displayTime, DateTime date)
        {
            DisplayTime = displayTime;
            Date = date;
        }

        public override string ToString()
        {
            return DisplayTime;
        }

        public DateTime GetDate()
        {
            return Date;
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
