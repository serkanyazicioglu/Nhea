using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Text;
using Nhea.Helper;

namespace Nhea.Data.Repository.EntityFrameworkRepository
{
    public abstract class BaseDbRepository<T> : Nhea.Data.Repository.EntityFrameworkRepository.BaseEntityFrameworkRepository<T>, IDisposable where T : class, new()
    {
        protected abstract System.Data.Entity.DbContext CurrentDbContext { get; }

        private System.Data.Objects.ObjectContext currentContext;
        protected override System.Data.Objects.ObjectContext CurrentContext
        {
            get
            {
                if (currentContext == null)
                {
                    currentContext = ((IObjectContextAdapter)CurrentDbContext).ObjectContext;
                    currentContext.ContextOptions.LazyLoadingEnabled = true;
                    currentContext.ContextOptions.ProxyCreationEnabled = true;
                }

                return currentContext;
            }
        }

        public override bool IsNew(T entity)
        {
            if (CurrentDbContext.Entry(entity).State == System.Data.EntityState.Added)
            {
                return true;
            }

            return false;
        }

        public override T GetById(object id)
        {
            try
            {
                EdmMember key = CurrentObjectSet.EntitySet.ElementType.KeyMembers.SingleOrDefault();

                if (key != null)
                {
                    CurrentContext.MetadataWorkspace.LoadFromAssembly(typeof(T).Assembly);



                    EntityKey e = new EntityKey(this.CurrentContext.DefaultContainerName + "." + typeof(T).Name, key.Name, ConvertionHelper.GetConvertedValue(id, ((System.Data.Metadata.Edm.PrimitiveType)(key.TypeUsage.EdmType)).ClrEquivalentType));

                    return (T)this.CurrentContext.GetObjectByKey(e);
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
