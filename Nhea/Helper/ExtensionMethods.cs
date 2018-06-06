using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nhea.Globalization;
using Nhea.Text;

namespace Nhea.Helper
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// A String formatter method which is localized by global data format.
        /// </summary>
        /// <param name="o">Object to format.</param>
        /// <param name="dataFormatType">Globalized data format.</param>
        /// <returns></returns>
        public static string ToString(this object o, DataFormatType dataFormatType)
        {
            return DataFormatManager.FormatToString(o, dataFormatType);
        }

        /// <summary>
        /// An object converter method which converts any type to another type. If value is null default value of type returns. In failure throws exception.
        /// </summary>
        /// <typeparam name="T">Targey type to convert.</typeparam>
        /// <param name="o">Object to be converted.</param>
        /// <returns></returns>
        public static T Convert<T>(this object o)
        {
            return ConvertionHelper.GetConvertedValue<T>(o);
        }

        /// <summary>
        /// An object converter method which converts any type to another type. If value is null default value of type returns. In failure returns default value.
        /// </summary>
        /// <typeparam name="T">Targey type to convert.</typeparam>
        /// <param name="o">Object to be converted.</param>
        /// <param name="defaultValue">In any type of failure this method returns this value.</param>
        /// <returns></returns>
        public static T Convert<T>(this object o, T defaultValue)
        {
            return ConvertionHelper.GetConvertedValue<T>(o, defaultValue);
        }

        /// <summary>
        /// An object converter method which converts any type to another type. If value is null default value of type returns. In failure returns default value.
        /// </summary>
        /// <typeparam name="T">Targey type to convert.</typeparam>
        /// <param name="o">Object to be converted.</param>
        /// <param name="defaultValue">In any type of failure this method returns this value.</param>
        /// <param name="culture">Specific culture info for formatting.</param>
        /// <returns></returns>
        public static T Convert<T>(this object o, T defaultValue, string culture)
        {
            return ConvertionHelper.GetConvertedValue<T>(o, defaultValue, culture);
        }

        /// <summary>
        /// Removes the last character of this string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveLastCharacter(this string text)
        {
            return CharacterReplace.RemoveLastCharacter(text);
        }

        public static Int64 ToUnixTime(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return 0;
            }

            var epoc = new DateTime(1970, 1, 1);
            var delta = dateTime - epoc;

            return (long)delta.TotalSeconds;
        }

        public static DateTime FromUnixTime(this string dateString)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Int64.Parse(dateString)).ToLocalTime();
        }
    }
}
