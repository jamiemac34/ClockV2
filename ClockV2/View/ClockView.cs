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

        public void SetPresenter(ClockPresenter presenter)
        {
            this.presenter = presenter;
        }

        public void UpdateAlarmDisplay(string alarmMessage)
        {
            lblNextAlarm.Text = alarmMessage;
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

        
        private void BtnAddClick(object sender, EventArgs e)
        {
            presenter.OnBtnAddClick();

        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            presenter.OnBtnViewClick();
        }

        private void BtnLoadClick(object sender, EventArgs e)
        {

        }

        public void HandleAddFormClose(object sender, EventArgs e)
        {
            presenter.UpdateAlarmDisplay();

        }

        
    }
}
