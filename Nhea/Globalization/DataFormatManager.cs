using System;

namespace Nhea.Globalization
{
    public static class DataFormatManager
    {
        public static string FormatToString(object value, DataFormatType dataFormatType)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string format = GetDataFormatString(dataFormatType);

            switch (dataFormatType)
            {
                case DataFormatType.None:
                default:
                    return value.ToString();
                case DataFormatType.Currency:
                    {
                        if (decimal.TryParse(value.ToString(), out decimal parsedValue))
                        {
                            return string.Format(format, parsedValue);
                        }
                        break;
                    }
                case DataFormatType.Date:
                case DataFormatType.DateTime:
                case DataFormatType.Time:
                    {
                        if (DateTime.TryParse(value.ToString(), out DateTime parsedValue))
                        {
                            return string.Format(format, parsedValue);
                        }
                        break;
                    }
                case DataFormatType.Double:
                    {
                        if (double.TryParse(value.ToString(), out double parsedValue))
                        {
                            return string.Format(format, parsedValue);
                        }
                        break;
                    }
                case DataFormatType.Integer:
                    {
                        if (int.TryParse(value.ToString(), out int parsedValue))
                        {
                            return string.Format(format, parsedValue);
                        }
                        break;
                    }
            }

            return value.ToString();
        }

        public static string GetDataFormatString(DataFormatType dataFormatType)
        {
            switch (dataFormatType)
            {
                case DataFormatType.None:
                default:
                    return "{0}";
                case DataFormatType.Currency:
                    return "{0:N2}";
                case DataFormatType.Date:
                    return "{0:" + System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern + "}";
                case DataFormatType.DateTime:
                    return "{0:" + System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern + "}";
                case DataFormatType.Double:
                    return "{0}";
                case DataFormatType.Integer:
                    return "{0:0}";
                case DataFormatType.Time:
                    return "{0:" + System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern + "}";
            }
        }

        public static string GetOfficeDataFormat(DataFormatType dataFormatType)
        {
            switch (dataFormatType)
            {
                case DataFormatType.None:
                default:
                    return "";
                case DataFormatType.Currency:
                    return @"\#\.\#\#0\,00";
                case DataFormatType.Date:
                    return System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                case DataFormatType.DateTime:
                    return System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                case DataFormatType.Double:
                    return @"0\,00";
                case DataFormatType.Integer:
                    return "";
                case DataFormatType.Time:
                    return System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
            }
        }

        public static string GetExportDataFormat(DataFormatType dataFormatType)
        {
            switch (dataFormatType)
            {
                default:
                case DataFormatType.None:
                    return "exportTextFormat";
                case DataFormatType.Double:
                    return "exportDoubleFormat";
                case DataFormatType.Currency:
                    return "exportCurrencyFormat";
                case DataFormatType.Date:
                    return "exportDateFormat";
                case DataFormatType.Time:
                    return "exportTimeFormat";
            }
        }
    }
}
