using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lgis
{
    public interface ILDataFilter
    {
         IEnumerable<DataRow> Filter(IEnumerable<DataRow> src);
    }
    public class LNumericDataFilter : ILDataFilter
    {
        public enum FilterType { LargerThan, LessThan, EqualTo };
        public DataColumn Column;
        public FilterType CompareType = FilterType.EqualTo;
        public object value;
        public IEnumerable<DataRow> Filter(IEnumerable<DataRow> src)
        {
            switch (CompareType)
            {
                case FilterType.EqualTo:
                    return (from row in src
                              where Compare(row[Column] , value)==0
                              select row);
                case FilterType.LargerThan:
                    return (from row in src
                              where Compare(row[Column] , value) > 0
                              select row);
                case FilterType.LessThan:
                    return (from row in src
                              where Compare(row[Column] , value) < 0
                              select row);
                default:
                    return null;
            }
        }
        int Compare(object a, object b)
        {
            IComparable compVar = a as IComparable;
            return compVar.CompareTo(b);
        }
    }
}
