using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate(int Id, string Name, decimal Price);  // recor tanımladığımız için aşağıdaki gibi tanımlayacaktır (init;) 
    //{
    //    public int Id { get; init; } // değeri nesne oluşturlurken verilmeli, dto lar readonly olmalı
    //    public string Name { get; init; }
    //    public decimal Price { get; init; }
    //}
}
