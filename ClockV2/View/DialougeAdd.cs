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
        private ReverseSortedArray<Person> alarmQueue;

        public DialougeAdd(ReverseSortedArray<Person> alarmQueue)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            DTPicker.MinDate = DateTime.Now;
            DTPicker.CustomFormat = "yyyy/MM/dd @ HH:mm:ss";
            

        }

        private void btnFormAddClick(object sender, EventArgs e)
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

            if (comDT >= 1)
            {
                MessageBox.Show("Alarm cannot be set to a past time", "Alarm Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                alarmQueue.Add(new Person(DTPicker.Text.ToString()), (int)epochTime.TotalSeconds);
                this.Close();
            }



        }

        
    }
}
