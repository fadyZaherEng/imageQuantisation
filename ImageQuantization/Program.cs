using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImageQuantization
{
    static class Program
    {
      //  public static int time;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
     //       time = Environment.TickCount;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
      //      time = Environment.TickCount - time;

        }
    }
}