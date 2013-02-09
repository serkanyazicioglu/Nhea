using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Enumeration
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class Detail : Attribute
    {
        private string value;

        public Detail(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }
}
