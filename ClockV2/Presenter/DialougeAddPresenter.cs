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
    /// <summary>
    /// Presenter class for the Add Alarm dialog.
    /// </summary>
    public class DialougeAddPresenter
    {
        private readonly DialougeAdd view;
        private readonly ReverseSortedArray<AlarmTime> alarmQueue;

        /// <summary>
        /// Constructor for the DialougeAddPresenter class.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="alarmQueue">The ReverseSortedArray holding the alarms</param>
        public DialougeAddPresenter(DialougeAdd view, ReverseSortedArray<AlarmTime> alarmQueue)
        {
            this.view = view;
            this.alarmQueue = alarmQueue;
        }

        /// <summary>
        /// Method to handle the click event of the "Add" button in the Add Alarm dialog.
        /// </summary>
        /// <param name="displayTime">The string taken from the datetime picker</param>
        /// <param name="selectedDT">The time taken from the datetime picker</param>
        /// <param name="selectedTriggerIndex">Used to pass the selected delay value into the alarm queue</param>
        /// <param name="name">The user-entered alarm name</param>
        /// <param name="description">The user-entered alarm description</param>
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

        /// <summary>
        /// Method to convert the selected index of the trigger time combo box into a TimeSpan.
        /// </summary>
        /// <param name="selectedIndex">Index corrosponding to a delay timespan, taken from the user's selection</param>
        /// <returns></returns>
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
