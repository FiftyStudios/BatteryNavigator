using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SmartBattery
{
    public partial class BatteryViewerMini : Form
    {
        private float lowBatteryLine = 20f;
        private float midBatteryLine = 50f;

        public BatteryViewerMini()
        {
            InitializeComponent();
        }

        const int WM_SYSCOMMAND = 0x112;
        const int SC_MOVE = 0xF010;

        [DllImport("User32.dll")]
        public static extern bool SetCapture(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(HandleRef hWnd,
        uint Msg, uint wParam, IntPtr lParam);

        private const uint WM_USER = 0x400;
        private const uint PBM_SETSTATE = WM_USER + 16;
        private const uint PBST_NORMAL = 0x0001;
        private const uint PBST_ERROR = 0x0002;
        private const uint PBST_PAUSED = 0x0003;

        private void BatteryViewerMini_Load(object sender, EventArgs e)
        {
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Question;
            progressBarBattery.Value = 100;
            timer1.Start();
            int left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int top = Screen.PrimaryScreen.WorkingArea.Height;
            DesktopBounds = new Rectangle(left, this.Height, this.Width, top);
            this.Size = new Size(113, 28);
        }

        private void UpdateFunction()
        {
            float batteryValue = SystemInformation.PowerStatus.BatteryLifePercent * 100f;

            progressBarBattery.Value = (int)batteryValue;

            SendMessage(new HandleRef(progressBarBattery, progressBarBattery.Handle),
            PBM_SETSTATE, PBST_NORMAL, IntPtr.Zero);

            if (batteryValue <= lowBatteryLine) {
                SendMessage(new HandleRef(progressBarBattery, progressBarBattery.Handle),
                PBM_SETSTATE, PBST_ERROR, IntPtr.Zero);
            }
            else if (batteryValue <= midBatteryLine)
            {
                SendMessage(new HandleRef(progressBarBattery, progressBarBattery.Handle),
                PBM_SETSTATE, PBST_PAUSED, IntPtr.Zero);
            }
            else
            {
                SendMessage(new HandleRef(progressBarBattery, progressBarBattery.Handle),
                PBM_SETSTATE, PBST_NORMAL, IntPtr.Zero);
            }

            string batteryStr = batteryValue.ToString() + "%";
            label1.Text = batteryStr;

            PowerLineStatus acStatus = SystemInformation.PowerStatus.PowerLineStatus;
            switch (acStatus)
            {
                case PowerLineStatus.Offline:
                    iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Battery;
                    break;
                case PowerLineStatus.Online:
                    iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Plug;
                    break;
                case PowerLineStatus.Unknown:
                    iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.QuestionCircle;
                    break;
            }
        }

        private void ShowMenuMouse(object sender, MouseEventArgs e)
        {
            Point mp = MousePosition;
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                SetCapture(this.Handle);
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE | 2, 0);
            }
            if (e.Button == MouseButtons.Right) contextMenuStrip1.Show(mp);
        }

        
        private void BatteryViewerMini_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Size = new Size(113, 28);
            UpdateFunction();
            timer1.Interval = 1000;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ShowMouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
