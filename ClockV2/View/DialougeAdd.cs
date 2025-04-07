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
using PriorityQueue;

namespace ClockV2.View
{
    public partial class DialougeAdd : Form
    {
        private ReverseSortedArray<AlarmTime> alarmQueue;

        public DialougeAdd(ReverseSortedArray<AlarmTime> alarmQueue)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            DTPicker.MinDate = DateTime.Now;
            DTPicker.MaxDate = DateTime.Now.AddDays(21);
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

        private void BtnFormAddClick(object sender, EventArgs e)
        {
            // taken from https://stackoverflow.com/questions/911717/split-string-convert-tolistint-in-one-line
            var timeInt = DTPicker.Text.Replace(" @ ", "/").Replace(":", "/")
            .Split('/')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();

            DateTime selectedDT = new DateTime(timeInt[0], timeInt[1], timeInt[2], timeInt[3], timeInt[4], timeInt[5]);

            TimeSpan epochTime = selectedDT - new DateTime(1970, 1, 1);

            int comDT = DateTime.Compare(DateTime.Now, selectedDT);

            TimeSpan triggerTime = TimeSpan.Zero;
            switch (CBTriggerTime.SelectedIndex)
            {
                case 0:
                    triggerTime = TimeSpan.Zero;
                    break;
                case 1:
                    triggerTime = TimeSpan.FromMinutes(-5);
                    break;
                case 2:
                    triggerTime = TimeSpan.FromMinutes(-10);
                    break;
                case 3:
                    triggerTime = TimeSpan.FromMinutes(-15);
                    break;
                case 4:
                    triggerTime = TimeSpan.FromMinutes(-30);
                    break;
                case 5:
                    triggerTime = TimeSpan.FromHours(-1);
                    break;
                case 6:
                    triggerTime = TimeSpan.FromHours(-2);
                    break;
                case 7:
                    triggerTime = TimeSpan.FromHours(-6);
                    break;
                case 8:
                    triggerTime = TimeSpan.FromHours(-12);
                    break;
                case 9:
                    triggerTime = TimeSpan.FromDays(-1);
                    break;
                case 10:
                    triggerTime = TimeSpan.FromDays(-2);
                    break;
                case 11:
                    triggerTime = TimeSpan.FromDays(-3);
                    break;
                case 12:
                    triggerTime = TimeSpan.FromDays(-7);
                    break;
            }

            AlarmTime selectedAT = new AlarmTime(DTPicker.Text.ToString(), selectedDT, triggerTime, txtName.Text, txtDescription.Text);

            if (comDT >= 1)
            {
                MessageBox.Show("Alarm cannot be set to a past time", "Alarm Error - Invalid Time",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (alarmQueue.Contains(selectedAT))
            {
                MessageBox.Show("An alarm is already set to that time", "Alarm Error - Already Set",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtName.Text == "Enter alarm name..." || txtDescription.Text == "Enter alarm description...")
            {
                MessageBox.Show("The name and description cannot be left blank.", "Alarm Error - Missing Name/Description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                alarmQueue.Add(selectedAT, (int)epochTime.TotalSeconds);
                this.Close();
            }



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
