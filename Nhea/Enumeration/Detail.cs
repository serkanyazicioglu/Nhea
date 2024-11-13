using System;

namespace Nhea.Enumeration
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class Detail : Attribute
    {
        public Detail(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}
