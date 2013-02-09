using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using System.Data.Objects.DataClasses;
using Nhea.Data;
using System.Linq.Expressions;
using Nhea.Enumeration;

namespace Nhea.Repository
{
    public abstract class Repository<T> : Nhea.Data.Repository.EntityFrameworkRepository.BaseDbRepository<T> where T : class, new()
    {
        private System.Data.Entity.DbContext currentDbContext;
        protected override System.Data.Entity.DbContext CurrentDbContext
        {
            get
            {
                if (currentDbContext == null)
                {
                    currentDbContext = new Nhea.Infrastructure.Data.NheaEntities();
                }

                return currentDbContext;
            }
        }
    }
}
