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
using System.Threading;
using System.Media;

namespace ClockV2
{
    public partial class ClockView : Form
    {
        private ClockPresenter presenter;
        private readonly ClockDrawingHelper drawingHelper = new ClockDrawingHelper();
        private DateTime currentTime;
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private AlarmTime alarmSet;
        private CancellationTokenSource alarmTokenSource;
        private NotifyIcon systemTray;



        public ClockView()
        {
            InitializeComponent();

            // Enable double buffering to avoid flicker
            Panel_Clock.GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(Panel_Clock, true, null);

            currentTime = DateTime.Now;
            alarmQueue = new ReverseSortedArray<AlarmTime>(99);

            systemTray = new NotifyIcon
            {
                Icon = this.Icon,
                Visible = true,
                Text = "ClockV2"
            };

            var trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Show", null, STMenuShowClick);
            trayMenu.Items.Add("Add Alarm", null, BtnAddClick);
            trayMenu.Items.Add("View Alarms", null, BtnViewClick);
            trayMenu.Items.Add("Exit", null, STMenuExitClick);

            systemTray.ContextMenuStrip = trayMenu;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        e.Cancel = true;
                        this.WindowState = FormWindowState.Minimized;
                        this.ShowInTaskbar = false;
                        this.Hide();
                    }
                }
            };

            systemTray.DoubleClick += (s, e) =>
            {
                this.ShowInTaskbar = true;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };

            this.Resize += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Hide();
                    this.ShowInTaskbar = false;
                }
            };
        }

        private void STMenuShowClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void STMenuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void STNotification(string title, string message)
        {
            systemTray.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
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

        private void BtnAddClick(object sender, EventArgs e)
        {
            var formPopup = new DialougeAdd(alarmQueue);
            formPopup.FormClosed += HandleAddFormClose;
            formPopup.Show(this);
        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            var formPopup = new DialougeView(alarmQueue, () => alarmTokenSource?.Cancel(), UpdateAlarmDisplay);
            formPopup.FormClosed += HandleAddFormClose;
            formPopup.Show(this);
        }

        private void BtnLoadClick(object sender, EventArgs e)
        {

        }

        public void HandleAddFormClose(object sender, EventArgs e)
        {
            UpdateAlarmDisplay();

        }

        public void UpdateAlarmDisplay()
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

            alarmTokenSource?.Cancel();
            alarmTokenSource = new CancellationTokenSource();

            try
            {

                await Task.Delay((int)alarmTime.GetDate().Subtract(DateTime.Now).TotalMilliseconds, alarmTokenSource.Token);
                if (alarmTokenSource.IsCancellationRequested)
                {
                    return;
                }
                alarmQueue.Remove();
                UpdateAlarmDisplay();
                string alarmMessage = $"Alarm triggered at {alarmTime.GetDate():HH:mm:ss}";
                systemTray.ShowBalloonTip(3000, "Alarm Triggered", alarmMessage, ToolTipIcon.Info);
            }
            catch (TaskCanceledException)
            {

            }

        }
    }
}
