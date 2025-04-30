using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClockV2.Alarm;
using ClockV2.Presenter;
using PriorityQueue;

namespace ClockV2.View
{
    /// <summary>
    /// This class represents a dialog for adding alarms.
    /// </summary>
    public partial class DialougeAdd : Form
    {
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private DialougeAddPresenter presenter;

        /// <summary>
        /// Constructor for the DialougeAdd class.
        /// </summary>
        /// <param name="alarmQueue"></param>
        public DialougeAdd(ReverseSortedArray<AlarmTime> alarmQueue)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            this.presenter = new DialougeAddPresenter(this, alarmQueue);
            DTPicker.MinDate = DateTime.Now;
            DTPicker.CustomFormat = "yyyy/MM/dd @ HH:mm:ss";

            CBTriggerTime.Items.AddRange(new object[]
            {
                "At time of event",
                "5 minutes before",
                "10 minutes before",
                "15 minutes before",
                "30 minutes before",
                "1 hour before",
                "2 hours before",
                "6 hours before",
                "12 hours before",
                "1 day before",
                "2 days before",
                "3 days before",
                "1 week before"
            });
            CBTriggerTime.SelectedIndex = 0;

            ToolTip triggerToolTip = new ToolTip();
            triggerToolTip.SetToolTip(CBTriggerTime, "Sets how long before the event the alarm will trigger.");
        }

        /// <summary>
        /// Handles displaying error messages.
        /// </summary>
        /// <param name="message"></param>
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Alarm Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles displaying warning messages.
        /// </summary>
        /// <param name="message"></param>
        public void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Alarm Error - Missing Name/Description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Handles the click event for the "Add" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFormAddClick(object sender, EventArgs e)
        {
            presenter.OnBtnFormAddClick(DTPicker.Text, DTPicker.Value, CBTriggerTime.SelectedIndex, txtName.Text, txtDescription.Text);
        }

        /// <summary>
        /// Handles the default text for the textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetPlaceholderText(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                if (textBox.Name == "txtName")
                {
                    textBox.Text = "Enter alarm name...";
                }
                else if (textBox.Name == "txtDescription")
                {
                    textBox.Text = "Enter alarm description...";
                }
                textBox.ForeColor = Color.Gray;

            }
        }

        /// <summary>
        /// Handles removing the placeholder text when the user focuses on the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && textBox.ForeColor == Color.Gray)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Handles applying 'RemovePlaceholderText' to the name textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtNameFocusGot(object sender, EventArgs e)
        {
            RemovePlaceholderText(sender, e);
        }

        /// <summary>
        /// Handles applying 'RemovePlaceholderText' to the description textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDescriptionFocusGot(object sender, EventArgs e)
        {
            RemovePlaceholderText(sender, e);
        }

        /// <summary>
        /// Handles applying 'SetPlaceholderText' to the name textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtNameFocusLost(object sender, EventArgs e)
        {
            SetPlaceholderText(sender, e);
        }

        /// <summary>
        /// Handles applying 'SetPlaceholderText' to the description textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtDescriptionFocusLost(object sender, EventArgs e)
        {
            SetPlaceholderText(sender, e);
        }


        /// <summary>
        /// Handles the load event for the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoad(object sender, EventArgs e)
        {
            SetPlaceholderText(txtName, e);
            SetPlaceholderText(txtDescription, e);
        }




    }
}
