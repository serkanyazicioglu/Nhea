using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nhea.Infrastructure.Data;

namespace Nhea.Repository
{
    public class ContactRepository : Repository<Contact>
    {
        public enum StatusType
        {
            Deleted = -1,
            [Detail("Okunmadı")]
            New = 0,
            [Detail("Okundu")]
            Read = 1,
            [Detail("Cevaplandı")]
            Replied = 2
        }

        public override System.Linq.Expressions.Expression<Func<Contact, object>> DefaultSorter
        {
            get
            {
                return contactQuery => new { contactQuery.CreateDate };
            }
        }

        protected override Nhea.Data.SortDirection DefaultSortType
        {
            get
            {
                return Nhea.Data.SortDirection.Descending;
            }
        }

        public override Contact CreateNew()
        {
            Contact contact = base.CreateNew();
            contact.Id = Guid.NewGuid();
            contact.CreateDate = DateTime.Now;
            contact.Status = StatusType.New.Convert<int>();

            return contact;
        }
    }
}
