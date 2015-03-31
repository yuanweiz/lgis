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
            Application.Run(new Form1());
        }
        static void Main(string[] args)
        {
            LUnitTest ut = new LUnitTest();
            //ut.TestPolyPolyLine();
            //ut.TestEnvelope();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //ut.TestLayerGroup();
            ut.TestList();
            Console.ReadLine();
        }
    }
}
