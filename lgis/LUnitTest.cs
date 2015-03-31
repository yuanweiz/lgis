using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Lgis
{
    public class LUnitTest
    {
        LPoint p = new LPoint(0, 1);
        LPolyline l1 = new LPolyline(), l2 = new LPolyline();
        LPolyPolyline ppl = new LPolyPolyline();
        public LUnitTest()
        {
            double[] array = new double[] { 1, 2, 3, 4, 5, 4, 3, 2 };
            for (int i = 0; i < array.Length; ++i)
            {
                l1.Add (new LPoint(i,array[i]));
                l2.Add (new LPoint(i+2, 10- array[i]));
            }
            ppl.Add(l1);
            ppl.Add(l2);
        }
        public void TestPolyLine()
        {
            LPoint p1 = new LPoint(3, 4);
            LPoint p2 = new LPoint(4, 5);
            LPolyline pl = new LPolyline();
            pl.Add(p1);
            pl.Add(p2);
            LEnvelope e = pl.Envelope;
            string s = e.ToString();
            Console.WriteLine("S=" + s);
            Console.WriteLine("p1's envelope" + p1.ToString());
        }
        public void TestPolyPolyLine()
        {
            Console.WriteLine("l1:");
            Console.WriteLine(l1.ToString());
            Console.WriteLine("Envelope of l1:"+l1.Envelope.ToString());
            Console.WriteLine("l2:");
            Console.WriteLine(l2.ToString());
            Console.WriteLine("Envelope of l2:"+l2.Envelope.ToString());
            LPolyPolyline ppl = new LPolyPolyline();
            ppl.Add(l1);
            ppl.Add(l2);
            Console.WriteLine("ppl:\n" + ppl.ToString());
            Console.WriteLine(ppl.Envelope.ToString());
        }
        public void TestEnvelope()
        {
            Console.WriteLine(l1.ToString());
            Console.WriteLine(l1.Envelope.ToString());
            l1.RemoveAt(0);
            Console.WriteLine(l1.ToString());
            Console.WriteLine(l1.Envelope.ToString());
            l1.RemoveAt(3);
            Console.WriteLine(l1.ToString());
            Console.WriteLine(l1.Envelope.ToString());
            Console.WriteLine(l2.ToString());
            Console.WriteLine(l2.Envelope.ToString());
            Console.WriteLine(ppl.Envelope.ToString());
            l2.RemoveAt(4);
            Console.WriteLine("Label");
            Console.WriteLine(ppl.Envelope.ToString());
            ppl.RemoveAt(0);
            Console.WriteLine(ppl.Envelope.ToString());
            ppl.RemoveAt(0);
            Console.WriteLine(ppl.Envelope.ToString());

        }
        public void TestWindow( Control c)
        {
            LWindow w = new LWindow(c, new LPoint(0, 0), 50);
        }
        public void TestLayerGroup() {
            LLayerGroup lg = new LLayerGroup();
            LVectorLayer vl = new LVectorLayer();
            LPolyline pl;
            vl.Add(p=new LPoint(1, 3));
            vl.Add(pl=new LPolyline());
            ((LPolyline)vl[1]).Add((LPoint)vl[0]);
            pl.Add(p);
            Console.WriteLine(vl[0].ToString());
            Console.WriteLine(vl[1].ToString());
        }
        /// <summary>
        /// It turns out that Copy() method
        /// can't be implemented via
        /// new List( Array)
        /// 
        /// </summary>
        public void TestList()
        {
            List<LPoint> lst1 = new List<LPoint>();
            List<LPoint> lst2 = new List<LPoint>();
            LPoint p = new LPoint(1, 3);
            lst1.Add(p);
            lst2 = new List<LPoint>(lst1.ToArray());
            p.X = 3;
            Console.WriteLine(lst2[0]);
        }

        public void TestEvent()
        {
        }

    }
}
