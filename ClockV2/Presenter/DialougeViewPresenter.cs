using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClockV2.Alarm;
using ClockV2.View;

namespace ClockV2.Presenter
{
    public class DialougeViewPresenter
    {
        private readonly DialougeView view;
        private readonly ReverseSortedArray<AlarmTime> alarmQueue;
        private readonly Action cancelAlarmCallback;
        private readonly Action updateAlarmDisplayCallback;

        public DialougeViewPresenter(DialougeView view, ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback)
        {
            this.view = view;
            this.alarmQueue = alarmQueue;
            this.cancelAlarmCallback = cancelAlarmCallback;
            this.updateAlarmDisplayCallback = updateAlarmDisplayCallback;
        }

        public void OnSaveAlarms()
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
                    view.ShowMessage("Alarms exported successfully!", "Export Successful", MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    view.ShowMessage($"Error saving file: {ex.Message}", "Error", MessageBoxIcon.Error);
                }
            }
        }

        public void OnRemoveAlarm(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                cancelAlarmCallback?.Invoke();
            }
            alarmQueue.RemoveViaIndex(selectedIndex);
            updateAlarmDisplayCallback.Invoke();
        }

        public void OnLBAlarmChange(int selectedIndex)
        {
            if (selectedIndex > -1)
            {
                view.EnableRemoveBtn(true);
            }
            else
            {
                view.EnableRemoveBtn(false);
            }
        }
    }

}
