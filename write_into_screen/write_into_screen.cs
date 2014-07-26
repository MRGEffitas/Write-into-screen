using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace write_into_screen
{
    class write_into_screen
    {

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [STAThread]
        public static void Main(string[] args)
        {
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            Thread.Sleep(5000);

            myScreenshot();
                      
        }

        private static void myScreenshot()
        {
            var thread = new Thread(ThreadStart);
            // allow UI with ApartmentState.STA though [STAThread] above should give that to you
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();

        }

        private static void ThreadStart()
        {
 
            SimulateUser.ReadAndApplyConfig(@"config.json");

        }
    }
}
