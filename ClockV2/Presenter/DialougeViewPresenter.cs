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
    /// <summary>
    /// Presenter for the DialougeView, handles the logic of the view.
    /// </summary>
    public class DialougeViewPresenter
    {
        private readonly DialougeView view;
        private readonly ReverseSortedArray<AlarmTime> alarmQueue;
        private readonly Action cancelAlarmCallback;
        private readonly Action updateAlarmDisplayCallback;
        public readonly Action OnSaveAlarms;

        /// <summary>
        /// Constructor for the DialougeViewPresenter class.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="alarmQueue">The ReverseSortedArray holding the alarms</param>
        /// <param name="cancelAlarmCallback">Callback to the asynced alarms, called on cancel</param>
        /// <param name="updateAlarmDisplayCallback">>Callback to the asynced alarms, called on update</param>
        /// <param name="OnSaveAlarms"></param>
        public DialougeViewPresenter(DialougeView view, ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback, Action OnSaveAlarms)
        {
            this.view = view;
            this.alarmQueue = alarmQueue;
            this.cancelAlarmCallback = cancelAlarmCallback;
            this.updateAlarmDisplayCallback = updateAlarmDisplayCallback;
            this.OnSaveAlarms = OnSaveAlarms;
        }


        /// <summary>
        /// Logic handling the removal of alarms from the listbox & queue.
        /// </summary>
        /// <param name="selectedIndex"></param>
        public void OnRemoveAlarm(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                cancelAlarmCallback?.Invoke();
            }
            alarmQueue.RemoveViaIndex(selectedIndex);
            updateAlarmDisplayCallback.Invoke();
        }

        /// <summary>
        /// Logic handling the accessibility of the remove button, disables whenever an alarm isn't selected.
        /// </summary>
        /// <param name="selectedIndex">The user's currently selected alarm</param>
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
