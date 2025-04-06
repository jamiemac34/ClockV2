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
            formPopup.FormClosed += updateAlarmDisplay;
            formPopup.Show(this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        public void updateAlarmDisplay(object sender, EventArgs e)
        {
            if (!(alarmQueue.Head() == alarmSet)) {
                alarmSet = alarmQueue.Head();
                lblNextAlarm.Text = "Next on " + alarmQueue.Head().GetDisplayTime();
                ScheduleAlarm(alarmQueue.Head());
            }
            
        }

        public async void ScheduleAlarm(AlarmTime alarmTime)
        {
            await Task.Delay((int)alarmTime.GetDate().Subtract(DateTime.Now).TotalMilliseconds);
            MessageBox.Show("This is a test", "Alarm Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
    }
}
