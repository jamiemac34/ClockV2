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
    public partial class DialougeAdd : Form
    {
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private DialougeAddPresenter presenter;

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

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Alarm Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Method to be called by the Presenter to show a warning message
        public void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Alarm Error - Missing Name/Description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnFormAddClick(object sender, EventArgs e)
        {
            presenter.OnBtnFormAddClick(DTPicker.Text, DTPicker.Value, CBTriggerTime.SelectedIndex, txtName.Text, txtDescription.Text);
        }

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

        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && textBox.ForeColor == Color.Gray)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        private void TxtNameFocusGot(object sender, EventArgs e)
        {
            RemovePlaceholderText(sender, e);
        }

        private void TxtDescriptionFocusGot(object sender, EventArgs e)
        {
            RemovePlaceholderText(sender, e);
        }

        private void TxtNameFocusLost(object sender, EventArgs e)
        {
            SetPlaceholderText(sender, e);
        }

        private void TxtDescriptionFocusLost(object sender, EventArgs e)
        {
            SetPlaceholderText(sender, e);
        }


        private void FormLoad(object sender, EventArgs e)
        {
            SetPlaceholderText(txtName, e);
            SetPlaceholderText(txtDescription, e);
        }




    }
}
