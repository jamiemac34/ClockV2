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
using PriorityQueue;

namespace ClockV2.View
{
    public partial class DialougeAdd : Form
    {
        private PriorityQueue<Person> alarmQueue;

        public DialougeAdd(PriorityQueue<Person> alarmQueue)
        {
            InitializeComponent();
            this.alarmQueue = alarmQueue;
            DTPicker.CustomFormat = "yyyy/MM/dd @ hh:mm:ss";
            

        }

        private void btnFormAddClick(object sender, EventArgs e)
        {
            // taken from https://stackoverflow.com/questions/911717/split-string-convert-tolistint-in-one-line
            var timeInt = DTPicker.Text.Replace(" @ ", "/").Replace(":", "/")
            .Split('/')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();

            TimeSpan epochTime = new DateTime(timeInt[0], timeInt[1], timeInt[2], timeInt[3], timeInt[4], timeInt[5]) - new DateTime(1970, 1, 1);

            alarmQueue.Add(new Person(DTPicker.Text.ToString()), (int)epochTime.TotalSeconds);

        }

        private void DialougeAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
