using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClockV2.Alarm;

namespace ClockV2.View
{
    public partial class DialougeView : Form
    {
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private Action cancelAlarmCallback;
        private Action updateAlarmDisplayCallback;

        public DialougeView(ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            this.cancelAlarmCallback = cancelAlarmCallback;
            this.updateAlarmDisplayCallback = updateAlarmDisplayCallback;
            alarmQueue.PopulateList(lbAlarms);
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            StringBuilder icsFile = new StringBuilder();

            icsFile.AppendLine("BEGIN:VCALENDAR");
            icsFile.AppendLine("VERSION:2.0");
            icsFile.AppendLine("CALSCALE:GREGORIAN");
            icsFile.AppendLine("PRODID:-//ClockV2SCAssingment//AlarmApp v1.0//EN");
            for (int i = 0; i <= alarmQueue.GetLength(); i++)
            {
                var alarm = alarmQueue.GetEntry(i).Item;
                icsFile.AppendLine(alarm.ToCalanderEvent());
            }
            icsFile.AppendLine("END:VCALENDAR");

            string icsFileClean = icsFile.ToString()
            .Replace("\r\n\r\n", "\r\n")
            .Trim();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "iCalendar Files|*.ics",
                Title = "Save Alarm Calendar"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, icsFileClean.ToString());
                    MessageBox.Show("Alarms exported successfully!", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            if (lbAlarms.SelectedIndex == 0)
            {
                cancelAlarmCallback?.Invoke();
                
            }
            alarmQueue.RemoveViaIndex(lbAlarms.SelectedIndex);
            lbAlarms.Items.RemoveAt(lbAlarms.SelectedIndex);
            updateAlarmDisplayCallback.Invoke();

        }

        private void LBAlarmsSelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbAlarms.SelectedIndex > -1)
            {
                btnRemove.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
            }
        }

       
    }
}
