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

        public DialougeView(ReverseSortedArray<AlarmTime> alarmQueue)
        {
            InitializeComponent();
            alarmQueue.populateList(lbAlarms);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show(lbAlarms.GetItemText(lbAlarms.SelectedIndex), "Alarm Trigger",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
