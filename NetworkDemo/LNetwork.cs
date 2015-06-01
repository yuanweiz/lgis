using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lgis;

namespace NetworkDemo
{
    public class LNetwork
    {
        LLineLayer Layer;
        private List<LLineseg> Linesegs = new List<LLineseg>();
        List<VertexRecord> Vertices = new List<VertexRecord>();

        public DataTable VertexTable = new DataTable();
        private DataTable LinesegTable = new DataTable();
        private class LinesegRecord
        {
            public LLineseg Lineseg;
            public List<VertexRecord> Crosspoints = new List<VertexRecord>();
        }
        private class VertexRecord 
        {
            /**************************
             * 
             *  A-----C---------------B
             *  0     r               1
             * 
             * **************************/
            public List<VertexRecord> AjasentVertex = new List<VertexRecord>();
            public Dictionary<LinesegRecord, double> Ratio = new Dictionary<LinesegRecord,double>();
            public LPoint Location;
        }
        

        public LNetwork(LLineLayer layer)
        {
            Layer = layer;
        }
        public void BuildGraph()
        {
            Linesegs.Clear();
            Vertices.Clear();
            foreach (LPolyline line in Layer.Lines)
                Linesegs.AddRange(line.Edges);
            VertexRecord crossPoint;
            int cnt = Linesegs.Count;
            for (int i = 0; i < cnt; ++i)
            {
                for (int j = i + 1; j < cnt; ++j)
                {
                    crossPoint = CrossPoint(Linesegs[i], Linesegs[j]);
                    if (crossPoint != null)
                        Vertices.Add(crossPoint);
                    
                }
            }
            //build topology relation


        }
        public IEnumerable<LPoint> CrossPoints
        {
            get
            {
                foreach (VertexRecord vertex in Vertices)
                    yield return vertex.Location;
            }
        }
        /****************************************************
         * \A                      vector outer products     
         *  \                       are used to judge 
         *   \  O                   whether the lineseg intersects  
         *D---\-------------C     if  ( (AD)x(AC) ) * ( (BD)x(BC) ) < 0     
         *     \                  then  AB are on different side     
         *      \ B               of CD , samely we can judge if CD are on diffrent        
         *                         side of AB            
         *                          if ( ****) <0 and (****) < 0 
         *                          then intersect (AB,CD) = true 
         * ********************/

        private VertexRecord CrossPoint (LLineseg x,LLineseg y){
            LEnvelope a = x.Envelope;
            LEnvelope b = y.Envelope;
            // if endian overlays
            /*
            Console.WriteLine(x.A.Info());
            Console.WriteLine(x.B.Info());
            Console.WriteLine(y.A.Info());
            Console.WriteLine(y.B.Info());
            */
            if (x.A == y.B ||
                x.B == y.A ||
                x.A == y.A ||
                x.B == y.B)
                return null;
            // when MBRs don't intersect
            if (a.XMin > b.XMax ||
                a.XMax < b.XMin ||
                a.YMin > b.YMax ||
                a.YMax < b.YMin)
                return null;
            // use inner product
            double[] prod = new double[4];
            LVector AD = y.B - x.A,
                BC = y.A - x.B,
                AC = y.A - x.A,
                BD = y.B - x.B;
            prod[0] = AD ^ AC;
            prod[1] = BD ^ BC;
            prod[2] = AD ^ BD;
            prod[3] = AC ^ BC;
            if ((AD ^ AC) * (BD ^ BC) > 0 || (AD ^ BD) * (AC ^ BC) > 0)
                return null;
            else
            {
                // AO = AB * prod[0]/(prod[0]-prod[1])
                // O = A + AO
                LVector AB = x.B - x.A;
                VertexRecord netVertex = new VertexRecord();
                double ratioAB = prod[0] / (prod[0] - prod[1]);
                double ratioCD = prod[2] / (prod[2] - prod[3]);
                netVertex.Location = x.A + (ratioAB) * AB;
                netVertex.Ratio.Add(x, ratioAB);
                netVertex.Ratio.Add(y, ratioCD);
                return netVertex;
            }
        }
    }

}
