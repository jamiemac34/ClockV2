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
        public readonly Action OnSaveAlarms;

        public DialougeViewPresenter(DialougeView view, ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback, Action OnSaveAlarms)
        {
            this.view = view;
            this.alarmQueue = alarmQueue;
            this.cancelAlarmCallback = cancelAlarmCallback;
            this.updateAlarmDisplayCallback = updateAlarmDisplayCallback;
            this.OnSaveAlarms = OnSaveAlarms;
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
