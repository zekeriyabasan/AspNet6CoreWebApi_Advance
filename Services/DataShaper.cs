using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties; // çalışma zamanında prop ları almak istiyoruz ama ctor da newlemeyi unutma
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); // reflaction ile prop ları çektik  | ve demek
        }
        public IEnumerable<ShapedEntity> GetShaperData(IEnumerable<T> entities, string stringFields)
        {
            var requiredFields = GetRequiredProperties(stringFields);
            return FetchDataForEntities(entities, requiredFields);
        }

        public ShapedEntity GetShaperData(T entity, string stringFields)
        {
            var requiredFields = GetRequiredProperties(stringFields);
            return FetchDataForEntity(entity, requiredFields);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string stringFields)
        {
           var requiredFields = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(stringFields))
            {
                var fields = stringFields.Split(',',StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = Properties.FirstOrDefault(p => p.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property is null)
                        continue;
                    requiredFields.Add(property);
                }
            }
            else
            {
                requiredFields = Properties.ToList();
            }

            return requiredFields;
        }

        private ShapedEntity FetchDataForEntity(T entity,IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapeObject = new ShapedEntity();

            foreach (var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapeObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id");
            shapeObject.Id = (int)objectProperty.GetValue(entity);

            return shapeObject;
 
        }

        private IEnumerable<ShapedEntity> FetchDataForEntities(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedDataObjects = new List<ShapedEntity>();

            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity,requiredProperties);
                shapedDataObjects.Add(shapedObject);
            }
            return shapedDataObjects;
        }
    }
}
