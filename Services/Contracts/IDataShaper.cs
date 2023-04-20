using Entities.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataShaper<T>
    {
        IEnumerable<ShapedEntity> GetShaperData(IEnumerable<T> entities, string stringFields);
        ShapedEntity GetShaperData(T entity, string stringFields);
    }
}
