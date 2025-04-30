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
    /// <summary>
    /// ClockView class represents the main view of the clock application.
    /// </summary>
    public partial class ClockView : Form
    {
        private ClockPresenter presenter;
        private readonly ClockDrawingHelper drawingHelper = new ClockDrawingHelper();
        private DateTime currentTime;
        private ReverseSortedArray<AlarmTime> alarmQueue;
        private AlarmTime alarmSet;
        private CancellationTokenSource alarmTokenSource;
        private NotifyIcon systemTray;


        /// <summary>
        /// Constructor for the ClockView class, also handles the logic to allow the app to minimise to the taskbar.
        /// </summary>
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
            trayMenu.Items.Add("Show Clock", null, STMenuShowClick);
            trayMenu.Items.Add("Add Alarm", null, BtnAddClick);
            trayMenu.Items.Add("View Alarms", null, BtnViewClick);
            trayMenu.Items.Add("Save Alarms", null, BtnSaveClick);
            trayMenu.Items.Add("Load Alarms", null, BtnLoadClick);
            trayMenu.Items.Add("Exit", null, STMenuExitClick);

            systemTray.ContextMenuStrip = trayMenu;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        presenter.OnExit();
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

        /// <summary>
        /// Sets the presenter for the ClockView.
        /// </summary>
        /// <param name="presenter"></param>
        public void SetPresenter(ClockPresenter presenter)
        {
            this.presenter = presenter;
        }

        /// <summary>
        /// Updates the display of the next alarm.
        /// </summary>
        /// <param name="alarmMessage"></param>
        public void UpdateAlarmDisplay(string alarmMessage)
        {
            lblNextAlarm.Text = alarmMessage;
        }

        /// <summary>
        /// Updates the clock display with the current time.
        /// </summary>
        /// <param name="currentTime"></param>
        public void UpdateClock(DateTime currentTime)
        {
            this.currentTime = currentTime;
            Panel_Clock.Invalidate(); // Trigger a redraw of the panel
        }

        /// <summary>
        /// Handles the paint event for the clock panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_Clock_Paint(object sender, PaintEventArgs e)
        {
            if (presenter == null) return;

            var g = e.Graphics;
            drawingHelper.DrawClock(g, currentTime, Panel_Clock.Width, Panel_Clock.Height);
        }

        /// <summary>
        ///  Handles the click event for the system tray menu to show the clock.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void STMenuShowClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        /// <summary>
        /// Handles the click event for the system tray menu to exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void STMenuExitClick(object sender, EventArgs e)
        {
            presenter.OnExit();
            Application.Exit();
        }

        /// <summary>
        /// Displays a notification in the system tray.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void STNotification(string title, string message)
        {
            systemTray.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }


        /// <summary>
        /// Handles the click event for the "Add" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddClick(object sender, EventArgs e)
        {
            presenter.OnBtnAddClick();

        }

        /// <summary>
        /// Handles the click event for the "View" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnViewClick(object sender, EventArgs e)
        {
            presenter.OnBtnViewClick();
        }

        /// <summary>
        ///  Handles the click event for the "Load" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoadClick(object sender, EventArgs e)
        {
            presenter.OnBtnLoadClick();
        }

        /// <summary>
        /// Handles the click event for the "Save" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleAddFormClose(object sender, EventArgs e)
        {
            presenter.UpdateAlarmDisplay();

        }

        /// <summary>
        /// Handles the load event for the ClockView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClockView_Load(object sender, EventArgs e)
        {
            presenter.OnBtnLoadClick();
        }

        /// <summary>
        /// Handles the click event for the "Save" button in the system tray menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, EventArgs e)
        {
            presenter.OnSaveAlarms();
        }

        /// <summary>
        /// Displays a message box with the specified message, title, and icon.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="icon"></param>
        public void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }
}
