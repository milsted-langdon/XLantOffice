using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XLantCore.Models
{
    public partial class DataMap
    {
        public static object MapRecord<TEntity>(List<DataMap> mapping, DataRow externalObject) where TEntity : class
        {
            object entity = default(TEntity);
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (DataMap map in mapping)
            {
                PropertyInfo internalProp = properties.Where(x => x.Name == map.InternalFieldName).FirstOrDefault();
                object value = Convert.ChangeType(externalObject[map.ExternalFieldName].ToString(), internalProp.PropertyType);
                internalProp.SetValue(entity, value);
            }
            return entity;
        }
    }
}
