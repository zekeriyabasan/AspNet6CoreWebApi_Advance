using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ShapedEntity
    {
        public int Id { get; set; }
        public Entity Entity { get; set; } // en sık yapılan hata ref tipli prop ın ref ını vermemek (ya ctor da yada tanımlandığı yerde verilmeli)
        public ShapedEntity()
        {
            Entity = new Entity();   
        }
    }
}
