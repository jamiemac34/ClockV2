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
            
            MessageBox.Show(lbAlarms.GetItemText(lbAlarms.SelectedIndex), "Alarm Trigger",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
