using ClockV2.Alarm;
using ClockV2.Model;
using ClockV2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

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
            var formPopup = new DialougeView(alarmQueue, () => alarmTokenSource?.Cancel(), UpdateAlarmDisplay);
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
    }
}
