using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ClockV2.Alarm;
using ClockV2.View;

namespace ClockV2.Presenter
{
    public class DialougeAddPresenter
    {
        private readonly DialougeAdd view;
        private readonly ReverseSortedArray<AlarmTime> alarmQueue;

        public DialougeAddPresenter(DialougeAdd view, ReverseSortedArray<AlarmTime> alarmQueue)
        {
            this.view = view;
            this.alarmQueue = alarmQueue;
        }

        public void OnBtnFormAddClick(string displayTime, DateTime selectedDT, int selectedTriggerIndex, string name, string description)
        {
            TimeSpan triggerTime = GetTriggerTimeFromSelection(selectedTriggerIndex);
            AlarmTime selectedAT = new AlarmTime(displayTime, selectedDT, triggerTime, name, description, "", null);

            if (DateTime.Compare(DateTime.Now, selectedDT) >= 1)
            {
                MessageBox.Show("Alarm cannot be set to a past time", "Alarm Error - Invalid Time",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (alarmQueue.Contains(selectedAT))
            {
                MessageBox.Show("An alarm is already set to that time", "Alarm Error - Already Set",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (name == "Enter alarm name..." || description == "Enter alarm description...")
            {
                MessageBox.Show("The name and description cannot be left blank.", "Alarm Error - Missing Name/Description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                alarmQueue.Add(selectedAT, (int)(selectedDT - new DateTime(1970, 1, 1)).TotalSeconds);
                view.Close();
            }

        }

        private TimeSpan GetTriggerTimeFromSelection(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 1: return TimeSpan.FromMinutes(-5);
                case 2: return TimeSpan.FromMinutes(-10);
                case 3: return TimeSpan.FromMinutes(-15);
                case 4: return TimeSpan.FromMinutes(-30);
                case 5: return TimeSpan.FromHours(-1);
                case 6: return TimeSpan.FromHours(-2);
                case 7: return TimeSpan.FromHours(-6);
                case 8: return TimeSpan.FromHours(-12);
                case 9: return TimeSpan.FromDays(-1);
                case 10: return TimeSpan.FromDays(-2);
                case 11: return TimeSpan.FromDays(-3);
                case 12: return TimeSpan.FromDays(-7);
                default: return TimeSpan.Zero;
            }
        }
    }
}
