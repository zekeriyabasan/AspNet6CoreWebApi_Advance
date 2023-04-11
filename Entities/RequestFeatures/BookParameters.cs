using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class BookParameters:RequestParameters
    {
        public uint MinPrice { get; set; } // uint price negatif değer alamaz
        public uint MaxPrice { get; set; } = 1000;
        public bool IsValid => MinPrice < MaxPrice;
        public string? SearchTerm { get; set; }
    }
}
