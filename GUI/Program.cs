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
        static void Main(string[] args)
        {
            LUnitTest ut = new LUnitTest();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmMain frmMain = new FrmMain();
            Application.Run(frmMain);
            Console.ReadLine();

        }
    }
}
