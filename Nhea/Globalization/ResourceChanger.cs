using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Nhea.Globalization
{
    public class ResourceChanger
    {
        public static object GetMultiValue(object key)
        {
            object result = key;

            try
            {
                object translate = HttpContext.GetGlobalResourceObject("DataTypes", key.ToString());

                if (translate != null)
                {
                    result = translate;
                }
            }
            catch
            { 
            }

            return result;
        }
    }
}
