﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.LogModels
{
    public class LogDetails
    {
        // propertyleri object olarak tanımlıyorum çünkü context üzerinde kullanacağız
        public Object? ModelName { get; set; } 
        public Object? Controller { get; set; }
        public Object? Action { get; set; }
        public Object? Id { get; set; }
        public Object CreatedAt { get; set; }

        public LogDetails()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public override string ToString()=> JsonSerializer.Serialize(this); 
        
    }
}
