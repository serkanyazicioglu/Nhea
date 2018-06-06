using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Nhea.Data.Repository
{
    public abstract class BaseRepository<T> : IDisposable, IRepository<T> where T : class, new()
    {
        public enum StatusType
        {
            Deleted = -1,
            Draft = 0,
            Available = 1
        }

        public virtual Expression<Func<T, bool>> DefaultFilter
        {
            get
            {
                return null;
            }
        }

        public virtual Expression<Func<T, object>> DefaultSorter
        {
            get
            {
                return null;
            }
        }

        protected virtual SortDirection DefaultSortType
        {
            get
            {
                return SortDirection.Ascending;
            }
        }

        #region CreateNew

        public virtual T CreateNew()
        {
            T entity = new T();

            this.Add(entity);

            return entity;
        }

        #endregion

        #region GetSingle

        public abstract T GetById(object id);

        public T GetSingle(Expression<Func<T, bool>> filter)
        {
            return GetSingleCore(filter, true);
        }

        public T GetSingle(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return GetSingleCore(filter, getDefaultFilter);
        }

        protected internal abstract T GetSingleCore(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        #endregion

        #region GetAll

        public IQueryable<T> GetAll()
        {
            int totalCount = 0;
            return GetAllCore(null, true, true, null, null, false, 0, 0, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            int totalCount = -1;
            return GetAllCore(filter, true, true, null, null, false, 0, 0, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            int totalCount = -1;
            return GetAllCore(filter, getDefaultFilter, true, null, null, false, 0, 0, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, bool getDefaultSorter)
        {
            int totalCount = -1;
            return GetAllCore(filter, getDefaultFilter, getDefaultSorter, null, null, false, 0, 0, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn)
        {
            int totalCount = -1;
            return GetAllCore(filter, getDefaultFilter, true, sortColumn, SortDirection.Ascending, false, 0, 0, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn, SortDirection sortDirection)
        {
            int totalCount = -1;
            return GetAllCore(filter, getDefaultFilter, true, sortColumn, sortDirection, false, 0, 0, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, int pageSize, int pageIndex, ref int totalCount)
        {
            return GetAllCore(filter, getDefaultFilter, true, null, null, true, pageSize, pageIndex, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn, SortDirection sortDirection, int pageSize, int pageIndex, ref int totalCount)
        {
            return GetAllCore(filter, getDefaultFilter, true, sortColumn, sortDirection, true, pageSize, pageIndex, ref totalCount);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn, SortDirection sortDirection, bool allowPaging, int pageSize, int pageIndex, ref int totalCount)
        {
            return GetAllCore(filter, getDefaultFilter, true, sortColumn, sortDirection, allowPaging, pageSize, pageIndex, ref totalCount);
        }

        protected internal abstract IQueryable<T> GetAllCore(Expression<Func<T, bool>> filter, bool getDefaultFilter, bool getDefaultSorter, string sortColumn, SortDirection? sortDirection, bool allowPaging, int pageSize, int pageIndex, ref int totalCount);

        #endregion

        #region Count & Any

        public int Count()
        {
            return CountCore(null);
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            return CountCore(filter);
        }

        protected abstract int CountCore(Expression<Func<T, bool>> filter);

        public bool Any(Expression<Func<T, bool>> filter)
        { 
            return AnyCore(filter);
        }

        protected abstract bool AnyCore(Expression<Func<T, bool>> filter);

        #endregion

        #region Add

        public abstract void Add(T entity);

        public abstract void Add(List<T> entities);

        #endregion

        #region IsNew

        public abstract bool IsNew(T entity);

        #endregion

        #region Save

        public abstract void Save();

        #endregion

        #region Delete

        public abstract void Delete(Expression<Func<T, bool>> filter);

        public abstract void Delete(T entity);

        #endregion

        #region Refresh

        public abstract void Refresh(T entity);

        #endregion

        public abstract void Dispose();
    }
}
