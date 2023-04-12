﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataShaper<T>
    {
        IEnumerable<ExpandoObject> GetShaperData(IEnumerable<T> entities, string stringFields);
        ExpandoObject GetShaperData(T entity, string stringFields);
    }
}
