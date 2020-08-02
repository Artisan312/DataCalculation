
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllDataWereAnalyzed
{
    public class PointList<T>:ObservableDataSource<T>
    {
        public int Count { get; set; }
        public void Add(T t)
        {
            if (this.Collection.Count > Count)
            {
                base.Collection.RemoveAt(0);
            }
            base.Collection.Add(t);
        }
    }
}