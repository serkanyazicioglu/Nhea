using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Data;

namespace Nhea.Data
{
    public static class RepositoryExtender
    {
        public static bool IsNew(this EntityObject entityObject)
        {
            if (entityObject != null)
            {
                return (entityObject.EntityState == EntityState.Added);
            }

            throw new Exception("Entity is null!");
        }
    }
}
