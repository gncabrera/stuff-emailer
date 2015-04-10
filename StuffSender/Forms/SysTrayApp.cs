using log4net;
using StuffSender.Forms;
using StuffSender.Forms.ProgressEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffSender
{
    public partial class SysTrayApp : Form
    {

        public ILog logger = LogManager.GetLogger(typeof(SysTrayApp));


        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private MenuItem infoMenuItem;
        private bool IsSenderOpened { get; set; }
        public SysTrayApp()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();

            infoMenuItem = new MenuItem("...");
            infoMenuItem.Enabled = false;
            trayMenu.MenuItems.Add(infoMenuItem);

            trayMenu.MenuItems.Add(new MenuItem("-"));
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Stuff Collector";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            //System tray events
            trayIcon.Click += trayIcon_Click;
            trayIcon.ShowBalloonTip(30, "Here I am", "Just click me or hit CTRL+SHIFT+R to send", ToolTipIcon.Info);

            logger.Info("Tray loaded!");
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing && (components != null))
            {
                components.Dispose();
            }

            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void trayIcon_Click(object sender, EventArgs e)
        {
            if (!IsSenderOpened)
            {
                var senderForm = new SenderForm
                {
                    SendActions = new SendActions
                    {
                        ProgressChange = (p, s) => SendProgress(p, s),
                        ProcessFinished = (s) => ProcessFinished(s),
                        ProcessStarted = (s) => ProcessStarted(s),
                        ProcessError = (exc) => ProcessError(exc)
                    }
                };
                IsSenderOpened = true;
                senderForm.Show();
                senderForm.FormClosed += SenderFormClosed;
            }
        }

        private void SenderFormClosed(object sender, FormClosedEventArgs e)
        {
            IsSenderOpened = false;
        }

        private void SendProgress(int progress, SendProgress sendProgress)
        {
            var title = "Send progress: " + progress + "%";
            var body = sendProgress.UploadedFile.FileName + " has been uploaded";
            trayIcon.ShowBalloonTip(10, title, body, ToolTipIcon.Info);

            if (sendProgress.NextFile == null)
                UpdateInfo("Sending email...");
            else
                UpdateInfo("Uploading " + sendProgress.NextFile.FileName + "...");
        }

        private void ProcessStarted(SendStart start)
        {
            var title = "Your stuff is being sent!";
            var body = start.Files.Any() ? start.Files.Count + " files are going to be uploaded!" : "No files are going to be uploaded!";
            trayIcon.ShowBalloonTip(10, title, body, ToolTipIcon.Info);

            if (start.Files.Any())
                UpdateInfo("Uploading " + start.Files.First().FileName + "...");
            else
                UpdateInfo("Sending email...");

        }

        private void ProcessFinished(SendFinished finished)
        {
            var title = "Your stuff has been sent";
            var body = "Just check in your inbox!";
            trayIcon.ShowBalloonTip(10, title, body, ToolTipIcon.Info);

            UpdateInfo("Stuff sent!");
        }

        private void ProcessError(Exception exception)
        {
            var title = "An error has ocurred";
            var body = exception.Message;
            trayIcon.ShowBalloonTip(30, title, body, ToolTipIcon.Error);

            UpdateInfo("An error has ocurred!");
        }

        private void UpdateInfo(string info)
        {
            infoMenuItem.Text = info;
        }
    }
}
