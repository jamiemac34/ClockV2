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
using ClockV2.Presenter;

namespace ClockV2.View
{
    public partial class DialougeView : Form
    {
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private DialougeViewPresenter presenter;

        public DialougeView(ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            this.presenter = new DialougeViewPresenter(this, alarmQueue, cancelAlarmCallback, updateAlarmDisplayCallback);
            alarmQueue.PopulateList(lbAlarms);
        }

        // Method to display alarms in the list box (called by the Presenter)
        public void DisplayAlarms()
        {
            alarmQueue.PopulateList(lbAlarms);
        }

        // Method to show a message (called by the Presenter for success/failure)
        public void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        // Method to handle saving the alarms to an ICS file
        private void BtnSaveClick(object sender, EventArgs e)
        {
            presenter.OnSaveAlarms();
        }

        // Method to handle removing an alarm
        private void BtnRemoveClick(object sender, EventArgs e)
        {
            presenter.OnRemoveAlarm(lbAlarms.SelectedIndex);
            lbAlarms.Items.RemoveAt(lbAlarms.SelectedIndex);
        }


        public void LBAlarmsSelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.OnLBAlarmChange(lbAlarms.SelectedIndex);
        }

        public void EnableRemoveBtn(bool cond)
        {
            btnRemove.Enabled = cond;
        }
    }
}
