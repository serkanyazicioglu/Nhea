﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nhea.Data
{
    public interface IRepository<T> where T : class, new()
    {
        #region CreateNew

        T CreateNew();

        #endregion

        #region GetSingle

        T GetById(object id);

        T GetSingle(Expression<Func<T, bool>> filter);

        T GetSingle(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        #endregion

        #region GetAll

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, bool getDeaultSorter);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn, SortDirection sortDirection);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, int pageSize, int pageIndex, ref int totalCount);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn, SortDirection sortDirection, int pageSize, int pageIndex, ref int totalCount);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, bool getDefaultFilter, string sortColumn, SortDirection sortDirection, bool allowPaging, int pageSize, int pageIndex, ref int totalCount);

        #endregion

        #region Count & Any

        int Count();

        int Count(Expression<Func<T, bool>> filter);

        bool Any(Expression<Func<T, bool>> filter);

        #endregion

        #region Add

        void Add(T entity);

        void Add(List<T> entities);

        #endregion

        #region IsNew

        bool IsNew(T entity);

        #endregion

        #region Save

        void Save();

        #endregion

        #region Delete

        void Delete(Expression<Func<T, bool>> filter);

        void Delete(T entity);

        #endregion

        #region Refresh

        void Refresh(T entity);

        #endregion
    }
}
