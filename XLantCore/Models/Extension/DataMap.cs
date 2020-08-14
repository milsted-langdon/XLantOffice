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
        /// <summary>
        /// Experimental - Allows an external csv object in a datatable to be mapped to a local object type
        /// </summary>
        /// <typeparam name="TEntity">The entity type we are mapping to</typeparam>
        /// <param name="mapping">the mapping list for that object type</param>
        /// <param name="externalObject">the data row containing the data for the object</param>
        /// <returns>an object of the tentity type</returns>
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
