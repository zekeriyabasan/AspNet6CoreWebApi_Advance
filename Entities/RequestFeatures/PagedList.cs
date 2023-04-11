using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PagedList<T>:List<T> //gelen itemlar list<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int pageNumber, int pageSize,int totalCount)
        {
            MetaData = new MetaData 
            {
                CurrentPage=pageNumber,
                PageSize=pageSize,  
                TotalCount=totalCount,
                TotalPage = (int)Math.Ceiling(totalCount/(double)pageSize)           
            };

            AddRange(items); // gelen itemlar ne ise onları PagedList e taşımış oluyoruz.
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source,int pageNumber,int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, pageNumber, pageSize, count);
        }
    }
}
