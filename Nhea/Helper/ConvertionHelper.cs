using System;
using System.ComponentModel;
using System.Globalization;
using Nhea.Enumeration;

namespace Nhea.Helper
{
    public static class ConvertionHelper
    {
        public static T GetConvertedValue<T>(object value, T defaultValue)
        {
            return GetConvertedValue(value, defaultValue, null);
        }

        public static T GetConvertedValue<T>(object value, T defaultValue, string culture)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue;
                }

                return GetConvertedValue<T>(value, culture);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T GetConvertedValue<T>(object value)
        {
            return GetConvertedValue<T>(value, null);
        }

        public static T GetConvertedValue<T>(object value, string culture)
        {
            try
            {
                if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
                {
                    return default;
                }
                else if ((typeof(T) == typeof(Guid)) || (typeof(T) == typeof(Guid?)) || (typeof(T).GetGenericArguments().Length > 0 && typeof(T).GetGenericArguments()[0].ToString() == "System.Guid"))
                {
                    Type conversionType = GetConvertionType(typeof(T));

                    return (T)Convert.ChangeType(new Guid(value.ToString()), conversionType);
                }
                else if (typeof(T).BaseType == typeof(Enum))
                {
                    return EnumHelper.GetEnum<T>(value);
                }
                else if (typeof(T) == typeof(bool))
                {
                    return (T)Convert.ChangeType(Convert.ToBoolean(value), typeof(T));
                }
                else
                {
                    return GetConvertedCultureValue<T>(value, culture);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Type", typeof(T).ToString());
                ex.Data.Add("Value", value);
                throw;
            }
        }

        public static object GetConvertedValue(object value, Type t)
        {
            return typeof(ConvertionHelper).GetMethod("GetConvertedValue", new Type[] { t }).MakeGenericMethod(t).Invoke(null, new object[] { value });
        }

        private static T GetConvertedCultureValue<T>(object value, string culture)
        {
            Type convertionType = GetConvertionType(typeof(T));

            if (typeof(T) == typeof(string))
            {
                value = value.ToString();
                return (T)(value);
            }

            if (value is IConvertible)
            {
                var cultureInfo = !string.IsNullOrEmpty(culture) ? new CultureInfo(culture) : System.Globalization.CultureInfo.CurrentCulture;

                value = (T)Convert.ChangeType(value, convertionType, cultureInfo);
            }
            return (T)value;
        }

        private static Type GetConvertionType(Type convertionType)
        {
            if (convertionType.IsGenericType && convertionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var nullableConverter = new NullableConverter(convertionType);
                convertionType = nullableConverter.UnderlyingType;
            }

            return convertionType;
        }
    }
}
