using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public abstract class RequestParameters // base class olarak tanımlayalım. Herkes için ortak (?pageNumber&pageSize)
    {
        const int maxPageSize = 50;

        // Auto-implemented property
        public int PageNumber { get; set; }

        // FULL-property
        public int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; } // gönderilen _pageSize max dan büyükse maxPageSize ı dön değilse kendisini
        
        }

    }
}
