using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(",");
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); // Nesnenin propertylerini alıyoruz reflaction için

            var orderByQueryBuilder = new StringBuilder();
            // name ascending, price descendind ... ,
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyParam = param.Split(' ')[0]; // boşluktan sonraki des asc den kurtul

                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyParam,
                    StringComparison.InvariantCultureIgnoreCase)); //2. parametre küçük büyük harf ayrımını görmezden gelmek istiyoruz. 

                if (objectProperty is null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderByQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            var orderQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' '); // sondaki virgülü kaldırarak ekle
            return orderQuery;
        }
    }
}
