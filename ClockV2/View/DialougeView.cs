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
    /// <summary>
    /// Represents the View for displaying and managing alarms.
    /// </summary>
    public partial class DialougeView : Form
    {
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private DialougeViewPresenter presenter;

        /// <summary>
        /// Constructor for the DialougeView class.
        /// </summary>
        /// <param name="alarmQueue"></param>
        /// <param name="cancelAlarmCallback"></param>
        /// <param name="updateAlarmDisplayCallback"></param>
        /// <param name="OnSaveAlarms"></param>
        public DialougeView(ReverseSortedArray<AlarmTime> alarmQueue, Action cancelAlarmCallback, Action updateAlarmDisplayCallback, Action OnSaveAlarms)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            this.presenter = new DialougeViewPresenter(this, alarmQueue, cancelAlarmCallback, updateAlarmDisplayCallback, OnSaveAlarms);
            alarmQueue.PopulateList(lbAlarms);
        }

        /// <summary>
        /// Handles printing the alarms to the listbox.
        /// </summary>
        public void DisplayAlarms()
        {
            alarmQueue.PopulateList(lbAlarms);
        }

        /// <summary>
        /// Handles displaying messages.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="icon"></param>
        public void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Handles the click event of the "Save" button in the View Alarm dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            presenter.OnSaveAlarms();
        }

        /// <summary>
        /// Handles the click event of the "Remove" button in the View Alarm dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveClick(object sender, EventArgs e)
        {
            presenter.OnRemoveAlarm(lbAlarms.SelectedIndex);
            lbAlarms.Items.RemoveAt(lbAlarms.SelectedIndex);
        }

        /// <summary>
        /// Handles passing the selected index of the listbox to the presenter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LBAlarmsSelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.OnLBAlarmChange(lbAlarms.SelectedIndex);
        }

        /// <summary>
        /// Enables or disables the remove button based on the condition provided.
        /// </summary>
        /// <param name="cond"></param>
        public void EnableRemoveBtn(bool cond)
        {
            btnRemove.Enabled = cond;
        }
    }
}
