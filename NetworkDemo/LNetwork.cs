﻿using System;
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
        private List<LinesegRecord> LinesegTable = new List<LinesegRecord>();
        List<VertexRecord> VertexTable = new List<VertexRecord>();

        //public DataTable VertexTable = new DataTable();
        //private DataTable LinesegTable = new DataTable();
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
            public List<int> AjacentVertices = new List<int>();
            public Dictionary<LinesegRecord, double> Ratio = new Dictionary<LinesegRecord,double>();
            public LPoint Location;
            public VertexRecord() { }
            public VertexRecord(LPoint p) { Location = p; }
        }
        

        public LNetwork(LLineLayer layer)
        {
            Layer = layer;

        }

        //ugly but useful
        LinesegRecord g_rec; // used to compare t value

        int RatioComp(VertexRecord x, VertexRecord y)
        {
            double xratio = x.Ratio[g_rec];
            double yratio = y.Ratio[g_rec];
            double delta = xratio - yratio;
            if (delta > 0)
                return 1;
            else if (delta < 0)
                return -1;
            else return 0;
        }
        public void BuildGraph()
        {
            #region init, add hang vertices into VertixTable
            LinesegTable.Clear();
            VertexTable.Clear();
            int cnt;
            foreach (LPolyline line in Layer.Lines)
            {
                cnt = line.Count;
                LLineseg lineseg ;
                LinesegRecord rec;
                // Handle edge case
                VertexRecord end = new VertexRecord(line[cnt - 1]);
                VertexRecord start = new VertexRecord(line[0]);
                lineseg = new LLineseg(line[0], line[1]);
                rec = new LinesegRecord();
                rec.Lineseg = lineseg;
                LinesegTable.Add(rec);
                start.Ratio.Add(rec, 0.0);
                rec.Crosspoints.Add(start);
                for (int i = 1; i < cnt - 1; ++i)
                {
                    lineseg= new LLineseg(line[i], line[i + 1]);
                    rec = new LinesegRecord();
                    rec.Lineseg = lineseg;
                    LinesegTable.Add(rec);
                }
                //now lineseg is ( line[cnt-2], line[cnt-1])
                rec.Crosspoints.Add(end);
                end.Ratio.Add(rec, 1.0);
                VertexTable.Add(start);
                VertexTable.Add(end);
            }
            #endregion

            #region calculate all crosspoints

            VertexRecord crossPoint;
            cnt = LinesegTable.Count;
            for (int i = 0; i < cnt; ++i)
            {
                for (int j = i + 1; j < cnt; ++j)
                {
                    crossPoint = CrossPoint(LinesegTable[i], LinesegTable[j]);
                    if (crossPoint != null)
                    {
                        LinesegTable[i].Crosspoints.Add(crossPoint);
                        LinesegTable[j].Crosspoints.Add(crossPoint);
                        VertexTable.Add(crossPoint);
                    }
                }
            }

            #endregion

            #region build topology relation
            foreach (LinesegRecord rec in LinesegTable)
            {
                List<VertexRecord> lst = rec.Crosspoints;
                g_rec = rec;
                lst.Sort(RatioComp);
                //Add ajacent vertices
                lst[0].AjacentVertices.Add(1);
                lst[lst.Count - 1].AjacentVertices.Add(lst.Count - 2);
                for (int i = 1; i < lst.Count - 1; ++i)
                {
                    lst[i].AjacentVertices.Add(i - 1);
                    lst[i].AjacentVertices.Add(i + 1);
                }
                
            }
            #endregion

        }
        public IEnumerable<LPoint> CrossPoints
        {
            get
            {
                foreach (VertexRecord vertex in VertexTable)
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

        private VertexRecord CrossPoint (LinesegRecord X,LinesegRecord Y){
            LLineseg x = X.Lineseg;
            LLineseg y = Y.Lineseg;
            LEnvelope a = x.Envelope;
            LEnvelope b = y.Envelope;
            // if endian overlays
            VertexRecord netVertex = new VertexRecord();
            LPoint endian;
            if ((endian = x.A) == y.B ||
                x.A == y.A ||
                (endian=x.B) == y.A ||
                x.B == y.B)
            {
                double r1, r2;
                netVertex.Location = endian;
                if (endian == x.A)
                    r1 = 0.0;
                else
                    r1 = 1.0;
                if (endian == y.A)
                    r2 = 0.0;
                else
                    r2 = 1.0;
                netVertex.Ratio.Add(X, r1);
                netVertex.Ratio.Add(Y, r2);
                return netVertex;
            }
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
                double ratioAB = prod[0] / (prod[0] - prod[1]);
                double ratioCD = prod[3] / (prod[3] - prod[2]);
                netVertex.Location = x.A + (ratioAB) * AB;
                netVertex.Ratio.Add(X, ratioAB);
                netVertex.Ratio.Add(Y, ratioCD);
                return netVertex;
            }
        }

        double[,] GetDistMatrix()
        {
            int cnt = VertexTable.Count;
            double [,]distmatrix=new double[cnt,cnt];
            for (int i = 0; i < cnt; ++i)
                for (int j = 0; j < cnt; ++j)
                    distmatrix[i, j] = -1.0;
            // notice that each edge is counted in twice,
            //but matrix is symmetric so it's not a waste of time
            for (int i = 0; i < cnt; ++i)
            {
                VertexRecord rec = VertexTable[i];
                for (int j=0;j<rec.AjacentVertices.Count ;++j){
                    int ajaidx = rec.AjacentVertices[j];
                    distmatrix[i, ajaidx] = distmatrix [ ajaidx,i]
                        = (VertexTable[i].Location - VertexTable[ajaidx].Location).Norm();
                }
            }
            return distmatrix;
        }
        private static bool ElementInArray(int elem, int[] array)
        {
            foreach (int val in array)
                if (val == elem)
                    return true;
            return false;
        }
        private static void AddToArray(int elem, int[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] < 0) { 
                    array[i] = elem;
                    return;
                }
            }
        }

        public int[] Dijkstra (int start,int end){
            int npoint = VertexTable.Count;
            int [] track = new int[npoint];
            if (start >= VertexTable.Count || start < 0
                || end >= VertexTable.Count || end <0)
            return null;
            double[,] distmatrix = GetDistMatrix();
            for (int i = 0; i < npoint; ++i)
            {
                for (int j = 0; j < npoint; ++j)
                    Console.Write(distmatrix[i,j].ToString() + " ");
                Console.WriteLine();
            }
            /***mindist[i] represents the minimum distance from start point to point i, so
             * it is trival that mindist[start] == 0
             **/
            return new int[1];
            double[] mindist = new double[npoint];
            /***track is a possible ( not the only one ) nearest path from start to end 
             * It's initialized to { -1 ,-1,...,-1},and during the process of algorithm,
             * points are added to the array list.
             * e.g.{1,4,3,2,-1} means the nearest path is 1->4->3->2 
             * */
            for (int i = 0; i < npoint; ++i)
            {
                track[i] = -1;
                mindist[i] = -1.0;
            }
            mindist[start] = 0;
            track[0] = start;
            while (mindist[end] < 0) //not calculated yet
            {
                double min = double.MaxValue;
                int newpoint = -1;
                for (int i = 0; i < npoint; ++i)
                {
                    if (!ElementInArray(i, track))
                        continue;
                    for (int j = 0; j < npoint; ++j)
                    {
                        if (ElementInArray(j, track) )
                            continue;
                        if ( distmatrix[i,j] >= 0 && min >= mindist[i] + distmatrix[i, j])
                        {
                            min = mindist[i] + distmatrix[i, j];
                            newpoint = j;
                        }
                    }
                }
                mindist[newpoint] = min;
                AddToArray(newpoint, track);
            }
            //return mindist[end];
            return track;
        }
        public void PrintInfo()
        {
            if (false)
            foreach (VertexRecord rec in VertexTable)
            {
                Console.WriteLine("Position:" + rec.Location.Info());
                foreach (KeyValuePair<LinesegRecord, double> pair in rec.Ratio)
                {
                    Console.WriteLine(pair.Key.Lineseg.Info()+
                        "Ratio:"+pair.Value.ToString());
                }
            }
            Console.WriteLine("Linesegs:");
            if (false)
            foreach (LinesegRecord rec in LinesegTable)
            {
                Console.WriteLine("Lineseg:" + rec.Lineseg.Info());
                foreach (VertexRecord crosspoint in rec.Crosspoints)
                    Console.WriteLine("CrossPoint:" + crosspoint.Location.Info());
            }
            if (false)
            foreach (LinesegRecord rec in LinesegTable)
            {
                Console.WriteLine(rec.Lineseg.Info());
                foreach (VertexRecord crosspoint in rec.Crosspoints)
                    ;
                    //Console.WriteLine(crosspoint.Ratio[rec]);
            }
            //print ajacent relation
            foreach(VertexRecord vertex in VertexTable){
                Console.WriteLine(vertex.Location.Info()+"'s ajacent vertices:");
                foreach (int idx in vertex.AjacentVertices)
                    Console.Write(VertexTable[idx].Location.Info() + " ");
                Console.WriteLine();
            }
        }
    }
}
