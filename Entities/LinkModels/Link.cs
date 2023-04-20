﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class Link
    {
        public string? Href { get; set; } // Hyper refrence
        public string? Rel { get; set; } // Relation
        public string? Method { get; set; }
        public Link() // serileştirme için gerekli
        {
            
        }
        public Link(string? href, string? rel, string? method) // full
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
