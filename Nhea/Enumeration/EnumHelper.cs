﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Nhea.Helper;

namespace Nhea.Enumeration
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns all information about the enum including; Name, Value and Detail.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        public static List<Enumeration> GetEnumerations<T>()
        {
            var enumerationList = new List<Enumeration>();

            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                foreach (Attribute attrib in field.GetCustomAttributes(true))
                {
                    Type underlyingType = Enum.GetUnderlyingType(typeof(T));
                    Detail detail = (Detail)attrib;

                    enumerationList.Add(new Enumeration
                    {
                        Name = field.GetValue(null).ToString(),
                        Value = ConvertionHelper.GetConvertedValue((Enum.Parse(typeof(T), field.GetValue(null).ToString())), underlyingType).ToString(),
                        Detail = detail.Value
                    });
                }
            }

            return enumerationList;
        }

        public static List<Enumeration> ConvertToEnumerations<T>(List<T> enumerations)
        {
            var enumerationList = new List<Enumeration>();

            foreach (var enumC in enumerations)
            {
                int enumcval = Convert.ToInt32(enumC);

                enumerationList.Add(new Enumeration
                {
                    Name = enumC.ToString(),
                    Value = enumcval.ToString(),
                    Detail = GetDetail<T>(enumcval)
                });
            }

            return enumerationList;
        }

        /// <summary>
        /// Returns the detail of an enum.
        /// </summary>
        /// <param name="e">Enumeration.</param>
        /// <returns></returns>
        public static string GetDetail(Enum e)
        {
            return Convert.ToString(typeof(EnumHelper).GetMethod("GetDetail", new Type[] { typeof(int) }).MakeGenericMethod(e.GetType()).Invoke(null, new object[] { Convert.ToInt32(e) }));
        }

        /// <summary>
        /// Returns the detail of an enum.
        /// </summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="value">Value of the enum.</param>
        /// <returns></returns>
        public static string GetDetail<T>(object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                return GetDetail<T>(Convert.ToInt32(value));
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the detail of an enum.
        /// </summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="value">Value of the enum.</param>
        /// <returns></returns>
        public static string GetDetail<T>(short value)
        {
            return GetDetail<T>(Convert.ToInt32(value));
        }

        /// <summary>
        /// Returns the detail of an enum.
        /// </summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="value">Value of the enum.</param>
        /// <returns></returns>
        public static string GetDetail<T>(int value)
        {
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                if (field.GetValue(null).ToString() == Enum.GetName(typeof(T), value))
                {
                    foreach (Attribute attrib in field.GetCustomAttributes(true))
                    {
                        Detail detail = (Detail)attrib;
                        return detail.Value;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns an enum for the given type and value.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        /// <param name="value">Name or value of the enum.</param>
        /// <returns></returns>
        public static T GetEnum<T>(object value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value.ToString());
            }
            catch (Exception ex)
            {
                Exception innerEx = new("En error occured while converting value to enumeration: " + ex.Message);
                innerEx.Data.Add("Type", typeof(T));
                innerEx.Data.Add("Value", value);
                throw innerEx;
            }
        }
    }
}
