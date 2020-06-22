﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nhea.Data.Repository
{
    public abstract class BaseRepository<T> : IDisposable, IRepository<T> where T : class, new()
    {
        public BaseRepository(bool isReadOnly = false)
        {
            this.IsReadOnly = isReadOnly;
        }

        public virtual bool IsReadOnly { get; set; }

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

        public abstract Task<T> GetByIdAsync(object id);

        public T GetSingle(Expression<Func<T, bool>> filter)
        {
            return GetSingleCore(filter, true);
        }

        public T GetSingle(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return GetSingleCore(filter, getDefaultFilter);
        }

        protected internal abstract T GetSingleCore(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            return await GetSingleCoreAsync(filter, true);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return await GetSingleCoreAsync(filter, getDefaultFilter);
        }

        protected internal abstract Task<T> GetSingleCoreAsync(Expression<Func<T, bool>> filter, bool getDefaultFilter);

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
            return CountCore(null, true);
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            return CountCore(filter, true);
        }

        public int Count(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return CountCore(filter, getDefaultFilter);
        }

        protected abstract int CountCore(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        public async Task<int> CountAsync()
        {
            return await CountCoreAsync(null, true);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await CountCoreAsync(filter, true);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return await CountCoreAsync(filter, getDefaultFilter);
        }

        protected abstract Task<int> CountCoreAsync(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return AnyCore(filter, true);
        }

        public bool Any(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return AnyCore(filter, getDefaultFilter);
        }

        protected abstract bool AnyCore(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await AnyCoreAsync(filter, true);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return await AnyCoreAsync(filter, getDefaultFilter);
        }

        protected abstract Task<bool> AnyCoreAsync(Expression<Func<T, bool>> filter, bool getDefaultFilter);

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

        public abstract Task SaveAsync();

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
