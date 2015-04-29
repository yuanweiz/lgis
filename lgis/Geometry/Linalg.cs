using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lgis;

namespace Lgis
{
    public class Matrix3D
    {
        double[,] matrix = {{0,0,0},{0,0,0},{0,0,0}};
        public Matrix3D() { }
        public Matrix3D(double[] arr)
        {
            if (arr == null || arr.Length != 9)
                throw new Exception("Matrix Size Mismatch");
            for (int i = 0; i < 9; ++i)
                matrix[i / 3, i % 3] = arr[i];
        }
        public double this[int a, int b]
        {
            get { return matrix[a, b]; }
            set { matrix[a, b] = value; }
        }
        public static Matrix3D operator +(Matrix3D a, Matrix3D b)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    ret[i, j] = a[i, j] + b[i, j];
            return ret;
        }

        public static Matrix3D operator *(Matrix3D a, Matrix3D b)
        {
            Matrix3D ret = new Matrix3D();
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    for (int k = 0; k < 3; ++k)
                        ret[i, j] += a[i, k] + b[k, j];
            return ret;
        }
        public static LPoint operator *(Matrix3D m, LPoint p)
        {
            double x = p.X, y = p.Y;
            LPoint ret = new LPoint();
            ret.X = m[0, 0] * x + m[0, 1] * y + m[0, 2] ;
            ret.Y = m[1, 0] * x + m[1, 1] * y + m[1, 2] ;
            return ret;
        }
        public static LVector operator *(Matrix3D m, LVector v)
        {
            double x = v.X, y = v.Y;
            LVector ret = new LVector();
            ret.X = m[0, 0] * x + m[0, 1] * y + m[0, 2] ;
            ret.Y = m[1, 0] * x + m[1, 1] * y + m[1, 2] ;
            return ret;
        }
    }

}
