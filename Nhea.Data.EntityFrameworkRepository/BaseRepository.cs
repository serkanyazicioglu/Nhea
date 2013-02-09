using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Text;
using Nhea.Helper;

namespace Nhea.Data.Repository.EntityFrameworkRepository
{
    public abstract class BaseRepository<T> : Nhea.Data.Repository.EntityFrameworkRepository.BaseEntityFrameworkRepository<T>, IDisposable where T : EntityObject, new()
    {
        public override T GetById(object id)
        {
            var keyProperty = GetPrimaryKeyInfo<T>();

            if (keyProperty != null)
            {
                try
                {
                    CurrentContext.MetadataWorkspace.LoadFromAssembly(typeof(T).Assembly);

                    EntityKey e = new EntityKey(this.CurrentContext.DefaultContainerName + "." + typeof(T).Name, keyProperty.Name, ConvertionHelper.GetConvertedValue(id, keyProperty.PropertyType));

                    return (T)this.CurrentContext.GetObjectByKey(e);
                }
                catch
                {
                }
            }

            return default(T);
        }

        private PropertyInfo GetPrimaryKeyInfo<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo pI in properties)
            {
                System.Object[] attributes = pI.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    if (attribute is EdmScalarPropertyAttribute)
                    {
                        if ((attribute as EdmScalarPropertyAttribute).EntityKeyProperty == true)
                            return pI;
                    }
                }
            }
            return null;
        }

        public override bool IsNew(T entity)
        {
            return (entity.EntityState == EntityState.Added);
        }
    }
}
