using ClockV2.Helpers;
using ClockV2.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClockV2.View;
using ClockV2.Alarm;
using PriorityQueue;
using System.Collections;

namespace ClockV2
{
    public partial class ClockView : Form
    {
        private ClockPresenter presenter;
        private readonly ClockDrawingHelper drawingHelper = new ClockDrawingHelper();
        private DateTime currentTime;
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private AlarmTime alarmSet;


        public ClockView()
        {
            InitializeComponent();

            // Enable double buffering to avoid flicker
            Panel_Clock.GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(Panel_Clock, true, null);

            currentTime = DateTime.Now;
            alarmQueue = new ReverseSortedArray<AlarmTime>(99);

        }

        public void SetPresenter(ClockPresenter presenter)
        {
            this.presenter = presenter;
        }

        public void UpdateClock(DateTime currentTime)
        {
            this.currentTime = currentTime;
            Panel_Clock.Invalidate(); // Trigger a redraw of the panel
        }

        private void Panel_Clock_Paint(object sender, PaintEventArgs e)
        {
            if (presenter == null) return;

            var g = e.Graphics;
            drawingHelper.DrawClock(g, currentTime, Panel_Clock.Width, Panel_Clock.Height);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var formPopup = new DialougeAdd(alarmQueue);
            formPopup.FormClosed += HandleAddFormClose;
            formPopup.Show(this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var formPopup = new DialougeView(alarmQueue);
            formPopup.FormClosed += HandleAddFormClose;
            formPopup.Show(this);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        public void HandleAddFormClose(object sender, EventArgs e)
        {
            updateAlarmDisplay();

        }

        public void updateAlarmDisplay()
        {
            if ((alarmQueue.IsEmpty()))
            {
                lblNextAlarm.Text = "No alarm set.";
            }
            else if (!(alarmQueue.Head() == alarmSet) && !(alarmQueue.IsEmpty()))
            {
                alarmSet = alarmQueue.Head();
                lblNextAlarm.Text = "Next on " + alarmQueue.Head().ToString();
                ScheduleAlarm(alarmQueue.Head());
            }
            
            
        }

        public async void ScheduleAlarm(AlarmTime alarmTime)
        {
            await Task.Delay((int)alarmTime.GetDate().Subtract(DateTime.Now).TotalMilliseconds);
            alarmQueue.Remove();
            updateAlarmDisplay();
            MessageBox.Show("This is a placeholder", "Alarm Trigger",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            

        }
    }
}
