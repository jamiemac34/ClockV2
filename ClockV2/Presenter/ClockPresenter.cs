using ClockV2.Alarm;
using ClockV2.Model;
using ClockV2.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ClockV2.Presenter
{
    public class ClockPresenter
    {
        private readonly ClockModel model;
        private readonly ClockView view;
        private readonly System.Timers.Timer timer;
        private readonly ReverseSortedArray<AlarmTime> alarmQueue;
        private CancellationTokenSource alarmTokenSource;


        public ClockPresenter(ClockModel model, ClockView view)
        {
            this.model = model;
            this.view = view;

            // Link the View to this Presenter
            view.SetPresenter(this);

            // Set up the timer for regular updates
            timer = new System.Timers.Timer(1000); // 1-second interval
            timer.Elapsed += (s, e) => UpdateClock();
            timer.Start();

            alarmQueue = new ReverseSortedArray<AlarmTime>(99);

        }

        private void UpdateClock()
        {
            // Fetch the current time from the Model
            DateTime currentTime = model.GetCurrentTime();

            // Update the View with the new time
            view.Invoke(new Action(() => view.UpdateClock(currentTime)));
        }

        public void OnBtnAddClick()
        {
            var formPopup = new DialougeAdd(alarmQueue);
            formPopup.FormClosed += (s, e) => UpdateAlarmDisplay();
            formPopup.Show();
        }

        public void OnBtnViewClick()
        {
            var formPopup = new DialougeView(alarmQueue, () => alarmTokenSource?.Cancel(), UpdateAlarmDisplay, OnSaveAlarms);
            formPopup.FormClosed += (s, e) => UpdateAlarmDisplay();
            formPopup.Show();
        }

        public void UpdateAlarmDisplay()
        {
            if (alarmQueue.IsEmpty())
            {
                view.UpdateAlarmDisplay("No alarm set.");
            }
            else
            {
                var nextAlarm = alarmQueue.Head();
                view.UpdateAlarmDisplay($"Next on {nextAlarm.ToString()}");
                ScheduleAlarm(nextAlarm);
            }
        }

        public async void ScheduleAlarm(AlarmTime alarmTime)
        {
            alarmTokenSource?.Cancel();
            alarmTokenSource = new CancellationTokenSource();


            if (!CheckForDelay(alarmTime))
            {
                Console.WriteLine($"Alarm scheduled for {alarmTime.GetDate():F}.");

                try
                {
                    await Task.Delay((int)alarmTime.GetDate().Subtract(DateTime.Now).TotalMilliseconds, alarmTokenSource.Token);
                    if (alarmTokenSource.IsCancellationRequested) return;

                    alarmQueue.Remove();
                    UpdateAlarmDisplay();

                    string alarmMessage = $"Alarm triggered at {alarmTime.GetDate():HH:mm:ss}";
                    view.STNotification("Alarm Triggered", alarmMessage);
                }
                catch (TaskCanceledException)
                {
                }

            }
            else 
            {
                Console.WriteLine($"Alarm for {alarmTime.GetDate():f} is more than 21 days away. Scheduling re-check in 20 days.");
                try
                {
                    await Task.Delay(TimeSpan.FromDays(20), alarmTokenSource.Token);
                    if (alarmTokenSource.IsCancellationRequested) return;
                    ScheduleAlarm(alarmTime);
                }
                catch (TaskCanceledException)
                {
                }

                return;
            }
            
        }

        public void OnBtnLoadClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "iCalendar files (*.ics)|*.ics";
            if (openFileDialog.ShowDialog() != DialogResult.OK) { return; }
            alarmQueue.Clear();

            string[] lines = File.ReadAllLines(openFileDialog.FileName);

            string uid = "";
            string name = "";
            string description = "";
            DateTime fileDT = DateTime.MinValue;
            DateTime setDate = DateTime.MinValue;
            TimeSpan triggerTime = TimeSpan.Zero;
            bool collecting = false;


            foreach (string line in lines)
            {
                if (line.StartsWith("BEGIN:VEVENT"))
                {
                    collecting = true;
                    uid = name = description = "";
                    fileDT = setDate = DateTime.MinValue;
                    triggerTime = TimeSpan.Zero;
                }
                else if (line.StartsWith("END:VEVENT") && collecting)
                {
                    collecting = false;
                    string displayTime = fileDT.ToString("g");
                    var fileAT = new AlarmTime(displayTime, fileDT, triggerTime, name, description, uid, setDate);
                    alarmQueue.Add(fileAT, (int)(fileDT - new DateTime(1970, 1, 1)).TotalSeconds);

                }
                else if (collecting)
                {
                    if (line.StartsWith("UID:"))
                        uid = line.Substring("UID:".Length);
                    else if (line.StartsWith("SUMMARY:") && !line.Contains("Trigger"))
                        name = line.Substring("SUMMARY:".Length);
                    else if (line.StartsWith("DESCRIPTION:") && !line.Contains("Trigger"))
                        description = line.Substring("DESCRIPTION:".Length);
                    else if (line.StartsWith("DTSTART:"))
                        fileDT = DateTime.ParseExact(line.Substring("DTSTART:".Length), "yyyyMMdd'T'HHmmss'Z'", null).ToLocalTime();
                    else if (line.StartsWith("DTSTAMP:"))
                        setDate = DateTime.ParseExact(line.Substring("DTSTAMP:".Length), "yyyyMMdd'T'HHmmss'Z'", null).ToLocalTime();
                    else if (line.StartsWith("TRIGGER:"))
                    {
                        string trigger = line.Substring("TRIGGER:".Length);
                        bool isNegative = trigger.StartsWith("-");
                        trigger = trigger.TrimStart('-', '+');
                        if (trigger.EndsWith("M"))
                            trigger = trigger.Substring(0, trigger.Length - 1);
                        if (int.TryParse(trigger, out int minutes))
                            triggerTime = TimeSpan.FromMinutes(isNegative ? -minutes : minutes);
                    }
                }

                UpdateAlarmDisplay();
            }
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

        public bool CheckForDelay(AlarmTime alarmTime)
        {

            TimeSpan timeUntilAlarm = alarmTime.GetDate() - DateTime.Now;
            Console.WriteLine($"Alarm Time: {alarmTime.GetDate()}");
            Console.WriteLine($"Time Until Alarm: {timeUntilAlarm.TotalMinutes} minutes");
            return timeUntilAlarm > TimeSpan.FromDays(21);
        }

        public void OnExit()
        {
            if (!alarmQueue.IsEmpty())
            {
                OnSaveAlarms();
            }
        }
    }
}
