using System;
using System.Media;
using System.Windows.Forms;
using DbMon.NET;


namespace Eximo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void DebugMonitorOnOutputDebugString(int pid, string text)
        {
            if (text.StartsWith("System.Windows.Data Error: BindingExpression path error:"))
            {
                DisplayMessage(text);
            }
        }

        private void ClearMessages()
        {
            Invoke((Action) delegate { rtfTrace.Text = ""; });
        }

        private void DisplayMessage(string message)
        {
            Invoke((Action) delegate
                                {
                                    SystemSounds.Exclamation.Play();
                                    rtfTrace.AppendText(String.Format("{0}\n", message));

                                    if (message.Length >= 64)
                                    {
                                        message = message.Substring(0, 60) + "...";
                                    }

                                    notifyIcon.Text = message;
                                    notifyIcon.BalloonTipText = message;
                                    notifyIcon.ShowBalloonTip(2000);
                                });
        }

        #region Event Handlers

        private void ClearToolStripMenuItemClick(object sender, EventArgs e)
        {
            ClearMessages();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            DebugMonitor.OnOutputDebugString += DebugMonitorOnOutputDebugString;
            DebugMonitor.Start();
        }

        private static void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Exit();
        }

        private static void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            Exit();
        }

        private static void Exit()
        {
            DebugMonitor.Stop();
            Application.Exit();
        }

        #endregion
    }
}