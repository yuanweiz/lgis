using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lgis
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new MainForm());
        //}
        static void Main(string[] args)
        {
            LUnitTest ut = new LUnitTest();
            //ut.TestPolyPolyLine();
            ut.TestEnvelope();
            Console.ReadLine();
        }
    }
}
