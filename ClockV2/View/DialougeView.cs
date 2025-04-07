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

        public DialougeView(ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            this.cancelAlarmCallback = cancelAlarmCallback;

            alarmQueue.populateList(lbAlarms);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show(lbAlarms.GetItemText(lbAlarms.SelectedIndex), "Alarm Trigger",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbAlarms.SelectedIndex == 0)
            {
                cancelAlarmCallback?.Invoke();
            }
            alarmQueue.removeViaIndex(lbAlarms.SelectedIndex);
            lbAlarms.Items.RemoveAt(lbAlarms.SelectedIndex);
            
        }

        private void lbAlarms_SelectedIndexChanged(object sender, EventArgs e)
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
