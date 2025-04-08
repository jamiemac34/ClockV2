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

        public DialougeView(ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback, Action OnSaveAlarms)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            this.presenter = new DialougeViewPresenter(this, alarmQueue, cancelAlarmCallback, updateAlarmDisplayCallback, OnSaveAlarms);
            alarmQueue.PopulateList(lbAlarms);
        }

        public void DisplayAlarms()
        {
            alarmQueue.PopulateList(lbAlarms);
        }

        public void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            presenter.OnSaveAlarms();
        }

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
