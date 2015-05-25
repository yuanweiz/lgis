using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Lgis;

namespace GUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void oldMain()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
        static void Main(string[] args)
        {
            LUnitTest ut = new LUnitTest();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
            Console.ReadLine();

        }
    }
}
