using System;
using System.Collections.Generic;
using System.Linq;
using Nhea.Data;

namespace Nhea.Data.Repository
{
    public abstract class ObjectRepository<T> : Nhea.Data.Repository.BaseRepository<T> where T : class, new()
    {
        public abstract List<T> CurrentList { get; set; }

        #region GetSingle

        public override T GetById(object id)
        {
            throw new NotImplementedException();
        }

        protected internal override T GetSingleCore(System.Linq.Expressions.Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            T item = default(T);

            if (filter != null)
            {
                item = CurrentList.SingleOrDefault(filter.Compile());
            }

            if (item == null)
            {
                item = CreateNew();
            }

            return item;
        }

        #endregion

        #region GetAll

        protected internal override IQueryable<T> GetAllCore(System.Linq.Expressions.Expression<Func<T, bool>> filter, bool getDefaultFilter, bool getDefaultSorter, string sortColumn, SortDirection? sortDirection, bool allowPaging, int pageSize, int pageIndex, ref int totalCount)
        {
            if (getDefaultFilter)
            {
                filter = filter.And(this.DefaultFilter);
            }

            if (filter == null)
            {
                filter = query => true;
            }

            IQueryable<T> returnList = CurrentList.Where(filter.Compile()).AsQueryable();

            if (!String.IsNullOrEmpty(sortColumn))
            {
                returnList = returnList.Sort(sortColumn, sortDirection);
            }
            else if (getDefaultSorter && DefaultSorter != null)
            {
                if (DefaultSortType == SortDirection.Ascending)
                {
                    returnList = returnList.OrderBy(DefaultSorter);
                }
                else
                {
                    returnList = returnList.OrderByDescending(DefaultSorter);
                }
            }

            if (allowPaging && pageSize > 0)
            {
                if (totalCount == 0)
                {
                    totalCount = returnList.Count();
                }

                int skipCount = pageSize * pageIndex;

                returnList = returnList.Skip<T>(skipCount).Take<T>(pageSize);
            }

            return returnList;
        }

        #endregion

        #region Count & Any

        protected override int CountCore(System.Linq.Expressions.Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return CurrentList.Count(filter.Compile());
        }

        protected override bool AnyCore(System.Linq.Expressions.Expression<Func<T, bool>> filter, bool getDefaultFilter)
        {
            return CurrentList.Any(filter.Compile());
        }

        #endregion

        #region Add

        public override void Add(T item)
        {
            CurrentList.Add(item);
        }

        public override void Add(List<T> items)
        {
            CurrentList.AddRange(items);
        }

        #endregion

        #region IsNew

        public override bool IsNew(T entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Save

        public override void Save()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Delete

        public override void Delete(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            foreach (T item in CurrentList.Where(filter.Compile()))
            {
                CurrentList.Remove(item);
            }
        }

        public override void Delete(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Refresh

        public override void Refresh(T entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override void Dispose()
        {
            CurrentList = null;
        }
    }
}
